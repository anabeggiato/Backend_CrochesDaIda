using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Products.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.ListProducts;

public class ListProducts : IEndpointDefinition 
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
        app.MapGet("/api/products", Handle);
    }

    private static IResult Handle(ProductStore store)
    {
        var products = store.Products.Values
            .OrderBy(product => product.Name)
            .Select(product => new Response(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.IsAvailable
            ))
            .ToList();
        
        return Results.Ok(products);
    }
}