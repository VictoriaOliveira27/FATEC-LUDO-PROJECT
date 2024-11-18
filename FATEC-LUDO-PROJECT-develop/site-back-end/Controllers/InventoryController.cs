using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class InventoryController: ControllerBase
{
    public LudoContext _ludocontext;
    public IMapper _mapper;

    public InventoryController(LudoContext ludoContext, IMapper mapper) 
    {
        _ludocontext = ludoContext;
        _mapper = mapper;
    }

    [HttpPost("list")]
    public IResult ListInventory ([FromBody] ListInventoryBody jsonBody)
    {
        // primeiro pego a lista de ids de cosmeticos que o usuario tem
        UserCosmetics cosmeticosEncontrados = _ludocontext.UserCosmetics.Where(x => x.user_id == jsonBody.UserId).FirstOrDefault();

        if (cosmeticosEncontrados == null)
        {
            return Results.Problem("inventario não foi encontrado");
        }

        List<string> listaDeIds = cosmeticosEncontrados.available_cosmetics;

        // depois com esses ids, capturo todos os cosmeticos da lista de cosmeticos
        List<Cosmetic> ListaGerada = _ludocontext.Cosmetics
            .Where(e => listaDeIds.Contains(e.id))
            .Skip(jsonBody.ItemsPerPage * jsonBody.Page)
            .Take(jsonBody.ItemsPerPage)
            .ToList();

        // filtra o resultado para voltar informações especificas do cosmético
        List<ListCosmeticsResponse> ListaFiltrada = _mapper.Map<List<ListCosmeticsResponse>>(ListaGerada); 
        return Results.Ok(ListaFiltrada);
    }

    [HttpGet("list_applied")]
    public IResult ListApplied ([FromBody] string userId)
    {
        //pega as informações de inventario do usuario
        var inventarioEncontrado = _ludocontext.UserCosmetics.Where(c => c.user_id == userId).FirstOrDefault();
        if(inventarioEncontrado == null)
        {
            return Results.Problem("inventario não encontrado");
        }

        List<string> listaDeIds = inventarioEncontrado.applied_cosmetics;

        // depois com esses ids, capturo todos os cosmeticos da lista de cosmeticos
        List<Cosmetic> ListaGerada = _ludocontext.Cosmetics.Where(e => listaDeIds.Contains(e.id)).ToList();
        List<ListCosmeticsResponse> ListaFiltrada = _mapper.Map<List<ListCosmeticsResponse>>(ListaGerada); 
        return Results.Ok(ListaFiltrada);
    }

    [HttpPost("apply")]
    public IResult ApplyCosmetic([FromBody] ApplyCosmeticBody jsonBody)
    {
        //pega as informações de inventario do usuario
        var inventarioEncontrado = _ludocontext.UserCosmetics.Where(c => c.user_id == jsonBody.Userid).FirstOrDefault();
        if(inventarioEncontrado == null)
        {
            return Results.Problem("inventario não encontrado");
        }
        //checa se o usuario possui o cosmético no seu inventario
        if (!inventarioEncontrado.available_cosmetics.Contains(jsonBody.CosmeticId))
        {
            return Results.Unauthorized();
        }
        //checa se o usuario ja tem o cosmetico aplicado 
        if (inventarioEncontrado.applied_cosmetics.Contains(jsonBody.CosmeticId))
        {
            return Results.Conflict("usuario ja aplicou esse cosmetico!");
        }
        //pega informações do cosmetico selecionado
        var cosmeticoEncontrado = _ludocontext.Cosmetics.Where(c => c.id == jsonBody.CosmeticId).FirstOrDefault();
        if (cosmeticoEncontrado == null)
        {
            return Results.NotFound("O cosmético a ser aplicado não foi encontrado!");
        }
        
        //checa se o usuario tem um cosmetico aplicado do mesmo tipo
        List<string> listaDeIds = inventarioEncontrado.applied_cosmetics;
        Cosmetic cosmeticosIgual = _ludocontext.Cosmetics.Where(c => c.item_type == cosmeticoEncontrado.item_type && listaDeIds.Contains(c.id)).FirstOrDefault();

        //se tiver, removemos ele da lista de cosmeticos aplicados
        if (cosmeticosIgual != null)
        {
            inventarioEncontrado.applied_cosmetics.Remove(cosmeticosIgual.id);   
        }
        //finalmente, aplicamos o cosmético ao usuário
        inventarioEncontrado.applied_cosmetics.Add(cosmeticoEncontrado.id);
        _ludocontext.UserCosmetics.Update(inventarioEncontrado);
        _ludocontext.SaveChanges();
        
        return Results.Ok();  
    }

    [HttpPost("unnapply")]
    public IResult UnnapplyCosmetic ([FromBody] ApplyCosmeticBody jsonBody)
    {
        //pega as informações de inventario do usuario
        var inventarioEncontrado = _ludocontext.UserCosmetics.Where(c => c.user_id == jsonBody.Userid).FirstOrDefault();
        if(inventarioEncontrado == null)
        {
            return Results.Problem("inventario não encontrado");
        }
        //checa se o cosmetico está aplicado ao usuario
        if(!inventarioEncontrado.applied_cosmetics.Contains(jsonBody.CosmeticId))
        {
            return Results.NotFound("esse cosmético não está aplicado");
        }

        //finalmente, remove o cosmeticos dos items aplicados
        inventarioEncontrado.applied_cosmetics.Remove(jsonBody.CosmeticId);
        _ludocontext.UserCosmetics.Update(inventarioEncontrado);
        _ludocontext.SaveChanges();

        return Results.Ok();  
    }
}