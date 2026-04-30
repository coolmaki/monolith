using Microsoft.AspNetCore.Routing;

namespace Monolith.Lib.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}