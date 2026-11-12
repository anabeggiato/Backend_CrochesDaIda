using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Features.Collections.Models;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Collections.UseCases.CreateCollection;

public class CreateCollection : IEndpointDefinition
{
    public record Request(
        string Name, 
        string Slug,
        string Description,
        bool IsActive
    );

    public record Response(
        Guid Id,
        string Name
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/collections", Handle);
    }

    private static async Task<IResult> Handle(
        Request request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest(
                "O nome da coleção é obrigatório"
            );
        }

        var collection = new Collection
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            IsActive = request.IsActive
        };

        dbContext.Collections.Add(collection);

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new Response(
            collection.Id,
            collection.Name
        );

        return Results.Created(
            $"/api/collections/{collection.Id}",
            response
        );
    }
}