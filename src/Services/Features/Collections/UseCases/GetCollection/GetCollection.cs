using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Collections.UseCases.GetCollection;

public class GetCollection : IEndpointDefinition
{
    public record Response(
        Guid Id,
        string Name,
        string Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/collections/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        Guid Id,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var collection = await db.Collections
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == Id,
                cancellationToken
            );
        
        if (collection is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new Response(
            collection.Id,
            collection.Name,
            collection.Description,
            collection.IsActive
        ));
    }
        
}