using Microsoft.AspNetCore.Routing;

namespace Shared.Endpoints;

public interface IEndpointDefiition
{
    void MapEndpoints(IEndpointRouteBuilder app);
}