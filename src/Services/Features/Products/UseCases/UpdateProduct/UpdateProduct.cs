using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Features.Products.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.UpdateProduct;

public class UpdateProduct : IEndpointDefinition
{
    public record Request(
        string? Name,
        string? Description,
        float Height,
        float Width,
        float Weight,
        decimal Price,
        bool IsAvailable,
        Guid CollectionId,
        Guid CategoryId
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

        if (request.Height <= 0)
        {
            return Results.BadRequest("A altura do produto deve ser maior que zero");
        }

        if (request.Width <= 0)
        {
            return Results.BadRequest("A largura do produto deve ser maior que zero");
        }

        if (request.Weight <= 0)
        {
            return Results.BadRequest("O peso do produto deve ser maior que zero");
        }

        var categoryExists = await dbContext.Categories
            .AnyAsync(category => category.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Results.BadRequest("A categoria informada não existe");
        }

        var product = await dbContext.Products.FindAsync(new object[] { id }, cancellationToken);

        if (product is null)
        {
            return Results.NotFound();
        }

        product.Name = name;
        product.Description = request.Description ?? string.Empty;
        product.Height = request.Height;
        product.Width = request.Width;
        product.Weight = request.Weight;
        product.Price = request.Price;
        product.IsAvailable = request.IsAvailable;
        product.CollectionId = request.CollectionId;
        product.CategoryId = request.CategoryId;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
