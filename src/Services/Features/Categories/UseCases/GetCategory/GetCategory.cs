using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Categories.UseCases.GetCategory;

public class GetCategory : IEndpointDefinition
{
    public record Response(
        Guid Id,
        string Name,
        string Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        Guid Id,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var category = await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == Id,
                cancellationToken
            );
        
        if (category is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new Response(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive
        ));
    }
    
}