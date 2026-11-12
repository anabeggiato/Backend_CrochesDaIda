using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.GetProducts;

public class GetProducts : IEndpointDefinition 
{
    public record Response(
        Guid Id,
        string Name,
        string Description,
        float Height,
        float Width,
        float Weight,
        decimal Price,
        bool IsAvailable,
        Guid CollectionId
    );

    public void MapEndpoints(IEndpointRouteBuilder app) 
    {
        app.MapGet("/api/products", Handle);
    }

    private static async Task<IResult> Handle(
        AppDbContext dbContext
    )
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Select(product => new Response(
                product.Id,
                product.Name,
                product.Description,
                product.Height,
                product.Width,
                product.Weight,
                product.Price,
                product.IsAvailable,
                product.CollectionId
            ))
            .ToListAsync();
        
        return Results.Ok(products);
    }
}