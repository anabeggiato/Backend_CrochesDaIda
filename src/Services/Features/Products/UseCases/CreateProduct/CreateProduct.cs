using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Products.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.CreateProduct;

public class CreateProduct : IEndpointDefinition
{
    public record Request(
        string Name,
        string Description,
        decimal Price,
        bool IsAvailable
    );

    public record Response(
        Guid Id,
        string Name,
        decimal Price
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", Handle);
    }

    private static async Task<IResult> Handle(
        Request request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest(
                "O nome do produto é obrigatório"
            );
        }
        if(request.Price <= 0)
        {
            return Results.BadRequest(
                "O preço deve ser maior que zero"
            );
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            IsAvailable = request.IsAvailable
        };

        dbContext.Products.Add(product);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new Response(
            product.Id,
            product.Name,
            product.Price
        );

        return Results.Created(
            $"/api/products/{product.Id}",
            response
        );
    }
}