using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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

    private static async Task<IResult> Handle(
        Guid id,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var product = await db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == id, 
                cancellationToken
            );

        if (product is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(product);
    }
}
