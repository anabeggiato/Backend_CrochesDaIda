using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Categories.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Categories.UseCases.CreateCategory;

public class CreateCategory : IEndpointDefinition
{
    public record Request(
        string Name,
        string Description,
        bool IsActive
    );

    public record Response(
        Guid Id,
        string Name
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", Handle);
    }

    private static async Task<IResult> Handle(
        Request request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest(
                "O nome da categoria é obrigatório"
            );
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        dbContext.Categories.Add(category);

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new Response(
            category.Id,
            category.Name
        );

        return Results.Created(
            $"/api/category/{category.Id}",
            response
        );
    }
}