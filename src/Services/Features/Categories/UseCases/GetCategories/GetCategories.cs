using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Categories.UseCases.GetCategories;

public class GetCategories : IEndpointDefinition
{
    public record Response(
        Guid Id,
        string Name,
        string Description,
        bool IsActive
    );

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories", Handle);
    }

    private static async Task<IResult> Handle(
        AppDbContext dbContext
    )
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new Response(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive
            ))
            .ToListAsync();

        return Results.Ok(categories);
    }
}