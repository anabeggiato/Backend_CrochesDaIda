using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Collections.UseCases.GetCollections;

public class GetCollections : IEndpointDefinition
{
    public record Response(
        Guid Id,
        string Name,
        string Slug,
        string Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/collections", Handle);
    }

    private static async Task<IResult> Handle(
        AppDbContext dbContext
    )
    {
        var collections = await dbContext.Collections
            .AsNoTracking()
            .OrderBy(collection => collection.Name)
            .Select(collection => new Response(
                collection.Id,
                collection.Name,
                collection.Slug,
                collection.Description,
                collection.IsActive
            ))
            .ToListAsync();
        
        return Results.Ok(collections);
    }
}