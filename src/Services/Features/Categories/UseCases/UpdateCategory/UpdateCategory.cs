using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Categories.UseCases.UpdateCategory;

public class UpdateCategory : IEndpointDefinition
{
    public record Request(
        string? Name,
        string? Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/categories/{id}", Handle);
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
            return Results.BadRequest("O nome da categoria é obrigatório");
        }

        var category = await dbContext.Categories.FindAsync(new object[] {id}, cancellationToken);

        if (category is null)
        {
            return Results.NotFound();
        }

        category.Name = name;
        category.Description = request.Description ?? string.Empty;
        category.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}