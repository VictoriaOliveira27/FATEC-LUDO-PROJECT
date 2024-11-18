using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LudoAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase 
{
    public LudoContext _ludocontext;
    public IMapper _mapper;

    public UserController(LudoContext ludoContext, IMapper mapper)
    {
        _ludocontext = ludoContext;
        _mapper = mapper;
    }

    [HttpPost("subscribe")]
    public IResult SubscribeUser([FromBody] SubscribeUserBody jsonBody)
    {

        //checando se já existe um usuário com username ou email
        var UsuariosParecidos = _ludocontext.Users.Where(u => u.username == jsonBody.Username || u.email == jsonBody.Email).ToList();

        //se existir retornar um Conflict 
        if(UsuariosParecidos.Count() > 0)
        {
            return Results.Conflict("email ou username já estão sendo usados!");
        }
        //se não existir realizar o cadastro do usuário
        else
        {
            var userid = Guid.NewGuid().ToString();
            User user = new User(){
                id = userid,
                username = jsonBody.Username,
                email = jsonBody.Email,
                password = jsonBody.Password,
                is_admin = false,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _ludocontext.Users.Add(user);
            
            UserCosmetics userCosmetics = new UserCosmetics(){
                user_id = userid,
                available_cosmetics = [],
                wishlist_cosmetics = [],
                applied_cosmetics = []
            };
            _ludocontext.UserCosmetics.Add(userCosmetics);

            _ludocontext.SaveChanges();
        }
        return Results.Created();
    }

    [HttpPost("login")]
    public IResult LoginUser ([FromBody] LoginUserBody jsonBody)
    {
        //procura se existe um usuário com tal senha e nome no banco
        var UsuarioEncontrado = _ludocontext.Users.Where(u => u.email == jsonBody.Username && u.password == jsonBody.Password).FirstOrDefault();
        if(UsuarioEncontrado == null)
        {
            return Results.Unauthorized();
        }

        var userToken = JwtService.GenerateJwtToken(UsuarioEncontrado.id);
        var userInfo = UsuarioEncontrado;
        return Results.Ok(UsuarioEncontrado);
    }

    // [Authorize]
    [HttpDelete("delete")]
    public IResult DeleteUser ([FromBody] String userId)
    {
        //procura o usuario pelo id informado
        var usuarioEncontrado = _ludocontext.Users.Where(u => u.id == userId).FirstOrDefault();
        if(usuarioEncontrado == null)
        {
            return Results.NotFound("usuário não foi encontrado");
        }
        //procura pelo seu inventario pessoal
        var inventarioEncontrado = _ludocontext.UserCosmetics.Where(u => u.user_id == userId).FirstOrDefault();
        if(inventarioEncontrado == null)
        {
            return Results.NotFound("inventario não foi encontrado");
        }

        //deleta usuario e seu inventario do banco
        _ludocontext.Users.Remove(usuarioEncontrado);
        _ludocontext.UserCosmetics.Remove(inventarioEncontrado);
        _ludocontext.SaveChanges();

        return Results.Ok();
    }

    [HttpGet("list")]
    public IResult ListUsers ([FromBody] ListUsersBody jsonBody) 
    {
        List<User> ListaGerada = _ludocontext.Users.Skip(jsonBody.ItemsPerPage * jsonBody.Page).Take(jsonBody.ItemsPerPage).ToList();
        // filtro o resultado para não voltar informações sensiveis do usuario, como a senha
        List<ListUsersResponse> ListaFiltrada = _mapper.Map<List<ListUsersResponse>>(ListaGerada); 
        return Results.Ok(ListaFiltrada);
    }

    [HttpGet("user_coins")]
    public IResult UserCoins ([FromQuery] String user_id)
    {
        var usuarioEncontrado = _ludocontext.Users.Where(u => u.id == user_id).FirstOrDefault();
        if(usuarioEncontrado == null)
        {
            return Results.NotFound("usuário não foi encontrado");
        }

        return Results.Ok(usuarioEncontrado.ludo_coins);
    }
}