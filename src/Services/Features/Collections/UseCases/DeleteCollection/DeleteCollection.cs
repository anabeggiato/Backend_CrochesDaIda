using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Collections.UseCases.DeleteCollection;

public class DeleteCollection : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/collections/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        string id,
        AppDbContext db,
        CancellationToken cancellationToken,
        ILogger<DeleteCollection> logger
    )
    {
        if (!Guid.TryParse(id, out var collectionId))
        {
            return Results.BadRequest("O id da coleção é inválido.");
        }

        var collection = await db.Collections.FindAsync(
            new object[] { collectionId },
            cancellationToken
        );

        if (collection is null)
        {
            return Results.NotFound("Coleção não encontrada.");
        }

        db.Collections.Remove(collection);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Erro ao excluir coleção {CollectionID}.", collectionId);
            return Results.Problem(
                title: "Não foi possível excluir a coleção",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Results.Ok(new {nessage = "Coleção excluída com sucesso."});
    }
}