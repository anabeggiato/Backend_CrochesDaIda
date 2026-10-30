using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Products.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.GetProduct;

public class GetProduct : IEndpointDefinition
{
    public record Response(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        bool IsAvailable
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id}", Handle);
    }

    private static IResult Handle(
        string id,
        ProductStore store)
    {
        if (!Guid.TryParse(id, out var productId))
            {
                return Results.BadRequest("ID inválido.");
            }
    
        if (!store.Products.TryGetValue(productId, out var product))
            {
                return Results.NotFound("Produto não encontrado");
            }

        var response = new Response(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.IsAvailable
        );

        return Results.Ok(response);
    }
}
