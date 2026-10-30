using Microsoft.AspNetCore.Routing;

namespace Shared.Endpoints;

public interface IEndpointDefinition
{
    void MapEndpoints(IEndpointRouteBuilder app);
}