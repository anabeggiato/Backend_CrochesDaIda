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
        decimal Price,
        bool IsAvailable
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
                product.Price,
                product.IsAvailable
            ))
            .ToListAsync();
        
        return Results.Ok(products);
    }
}