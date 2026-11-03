using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Products.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.UpdateProduct;

public class UpdateProduct : IEndpointDefinition
{
    public record Request(
        string? Name,
        string? Description,
        decimal Price,
        bool IsAvailable
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/products/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        Guid id,
        Request request,
        AppDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var name = request.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            return Results.BadRequest("O nome do produto é obrigatório");
        }

        if (request.Price <= 0)
        {
            return Results.BadRequest("O preço deve ser maior que zero");
        }

        var product = await dbContext.Products.FindAsync(new object[] { id }, cancellationToken);

        if (product is null)
        {
            return Results.NotFound();
        }

        product.Name = name;
        product.Description = request.Description ?? string.Empty;
        product.Price = request.Price;
        product.IsAvailable = request.IsAvailable;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
