using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Categories.UseCases.DeleteCategory;

public class DeleteCategory : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/categories/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        string Id,
        AppDbContext db,
        CancellationToken cancellationToken,
        ILogger<DeleteCategory> logger
    )
    {
        if (!Guid.TryParse(Id, out var categoryId))
        {
            return Results.BadRequest("O id da categoria é inválido.");
        }

        var category = await db.Categories.FindAsync(
            new object[] { categoryId },
            cancellationToken
        );

        if (category is null)
        {
            return Results.NotFound("Categoria não encontrada");
        }

        db.Categories.Remove(category);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Erro ao escluir categoria {categoryID}.", categoryId);
            return Results.Problem(
                title: "Não foi possível excluir a categoria",
                statusCode: StatusCodes.Status500InternalServerError    
            );
        }

        return Results.Ok(new {message = "Coleção excluída com sucesso."});
    }
}