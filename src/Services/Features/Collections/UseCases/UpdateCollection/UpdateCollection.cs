using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Collections.UseCases.UpdateCollection;

public class UpdateCollection : IEndpointDefinition
{
    public record Request(
        string? Name,
        string? Slug,
        string? Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("api/collections/{id}", Handle);
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
            return Results.BadRequest("O nome da coleção é obrigatório");
        }

        var collection = await dbContext.Collections.FindAsync(new object[] {id}, cancellationToken);

        if (collection is null)
        {
            return Results.NotFound();
        }

        collection.Name = name;
        collection.Slug = request.Slug ?? string.Empty;
        collection.Description = request.Description ?? string.Empty;
        collection.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}