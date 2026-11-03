using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Persistence;
using Shared.Endpoints;

namespace Services.Features.Products.UseCases.DeleteProduct;

public class DeleteProduct : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/products/{id}", Handle);
    }

    private static async Task<IResult> Handle(
        string id,
        AppDbContext db,
        CancellationToken cancellationToken,
        ILogger<DeleteProduct> logger
    )
    {
        if (!Guid.TryParse(id, out var productId))
        {
            return Results.BadRequest("O ID do produto é inválido.");
        }

        var product = await db.Products.FindAsync(
            new object[] { productId },
            cancellationToken
        );

        if (product is null)
        {
            return Results.NotFound("Produto não encontrado.");
        }

        db.Products.Remove(product);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Erro ao excluir produto {ProductId}.", productId);
            return Results.Problem(
                title: "Não foi possível excluir o produto.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Results.Ok(new { message = "Produto excluído com sucesso." });
    }
}
