using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CosmeticController : ControllerBase 
{
    public LudoContext _ludocontext;
    public IMapper _mapper;

    public CosmeticController(LudoContext ludoContext, IMapper mapper)
    {
        _ludocontext = ludoContext;
        _mapper = mapper;
    }

    [HttpPost("subscribe")]
    public IResult SubscribeCosmetic([FromBody] SubscribeCosmeticBody jsonBody)
    {
        //checando se já existe um cosmético com mesmo nome
        var cosmeticosParecidos = _ludocontext.Cosmetics.Where(u => u.name == jsonBody.Name).ToList();
        //se existir retornar um Conflict 
        if(cosmeticosParecidos.Count() > 0)
        {
            return Results.Conflict("nome já está sendo usado por outro cosmético");
        }
        else 
        {
            Cosmetic newCosmetic = new Cosmetic(){
                id = Guid.NewGuid().ToString(),
                name = jsonBody.Name,
                info = jsonBody.Info,
                price = jsonBody.Price,
                image = jsonBody.Image,
                data = jsonBody.Data,
                item_type = jsonBody.ItemType,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _ludocontext.Cosmetics.Add(newCosmetic);
            _ludocontext.SaveChanges();
        }
        return Results.Ok();
    }

    [HttpPost("buy")]
    public IResult BuyCosmetic([FromBody] BuyCosmeticBody jsonBody)
    {
        //pega informações do usuario
        var usuarioEncontrado = _ludocontext.Users.Where(u => u.id == jsonBody.Userid).FirstOrDefault();
        if (usuarioEncontrado == null)
        {
            return Results.Problem("usuario não encontrado");
        }
        //pega informações do cosmético
        var cosmeticoEncontrado = _ludocontext.Cosmetics.Where(u => u.id == jsonBody.CosmeticId).FirstOrDefault();
        if (cosmeticoEncontrado == null)
        {
            return Results.NotFound("cosmético não encontrado");
        }
        //pega informações do inventario do usuario
        var usuarioInventario = _ludocontext.UserCosmetics.Where(u => u.user_id == jsonBody.Userid).FirstOrDefault();
        if (usuarioInventario == null)
        {
            return Results.NotFound("inventario não encontrado");
        }

        //checa se o usuário já contem o cosmético em seu inventário (não pode ter cosméticos duplicados!)
        if (usuarioInventario.available_cosmetics.Contains(jsonBody.CosmeticId))
        {
            return Results.Conflict("usuário já tem esse cosmético");
        }

        //compara se o usuario tem saldo para comprar o cosmetico
        if(usuarioEncontrado.ludo_coins < cosmeticoEncontrado.price)
        {
            return Results.Unauthorized();    
        }

        //caso tenha saldo, adicionar cosmetico ao inventario e descontar o saldo do usuario
        usuarioInventario.available_cosmetics.Add(cosmeticoEncontrado.id);
        _ludocontext.UserCosmetics.Update(usuarioInventario);
        
        usuarioEncontrado.ludo_coins -= cosmeticoEncontrado.price; 
        _ludocontext.Users.Update(usuarioEncontrado);

        _ludocontext.SaveChanges();
        
        return Results.Ok();  
    }
    
    [HttpDelete("delete")]
    public IResult DeleteCosmetic ([FromBody] String cosmeticId)
    {
        //procura o cosmetico pelo id informado
        var cosmeticoEncontrado = _ludocontext.Cosmetics.Where(u => u.id == cosmeticId).FirstOrDefault();
        if(cosmeticoEncontrado == null)
        {
            return Results.NotFound("cosmetico não foi encontrado");
        }

        //procura pelos usuarios que já possuem o cosmetico
        var usuariosQueCompraram = _ludocontext.UserCosmetics.Where(u => u.available_cosmetics.Contains(cosmeticId)).ToList();
        foreach(var usuario in usuariosQueCompraram)
        {
            //realiza o reembolso do valor do cosmetico para o usuario
            var usuarioEncontrado = _ludocontext.Users.Where(u => u.id == usuario.user_id).FirstOrDefault();
            usuarioEncontrado.ludo_coins += cosmeticoEncontrado.price;

            //remove o item do inventario
            usuario.available_cosmetics.Remove(cosmeticId);

            _ludocontext.UserCosmetics.Update(usuario);
            _ludocontext.Users.Update(usuarioEncontrado);
        }

        //depois de tirar o cosmetico de todos os inventarios, deletar o cosmetico
        _ludocontext.Cosmetics.Remove(cosmeticoEncontrado);
        _ludocontext.SaveChanges();
        return Results.Ok();
    }

    [HttpGet("list")]
    public IResult ListCosmetics ([FromBody] ListCosmeticsBody jsonBody) 
    {
        List<Cosmetic> ListaGerada = _ludocontext.Cosmetics.Skip(jsonBody.ItemsPerPage * jsonBody.Page).Take(jsonBody.ItemsPerPage).ToList();
        // filtra o resultado para voltar informações especificas do cosmético
        List<ListCosmeticsResponse> ListaFiltrada = _mapper.Map<List<ListCosmeticsResponse>>(ListaGerada); 
        return Results.Ok(ListaFiltrada);
    }
}