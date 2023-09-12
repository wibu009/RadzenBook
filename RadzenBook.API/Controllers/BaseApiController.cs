using System.Net;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.API.Extensions;
using RadzenBook.Contract.Core;
using RadzenBook.Service.Interfaces.Features;

namespace RadzenBook.API.Controllers;

[ApiController]
[Route("api/[controller]s")]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{
    private IFeaturesServiceManager? _featureServiceManager;
    
    protected IFeaturesServiceManager FeatureServiceManager => _featureServiceManager ??= 
        (HttpContext.RequestServices.GetService(typeof(IFeaturesServiceManager)) as IFeaturesServiceManager)!;
    
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result switch
        {
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.BadRequest } => new BadRequestObjectResult(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Unauthorized } => new UnauthorizedObjectResult(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Forbidden } => new ForbidResult(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.NotFound } => new NotFoundObjectResult(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Conflict } => new ConflictObjectResult(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.InternalServerError } => new ObjectResult(result.Message) { StatusCode = (int)HttpStatusCode.InternalServerError },
            { IsSuccess: false, Message: not null } => new BadRequestObjectResult(result.Message),
            { IsSuccess: true, Value: not null } => new OkObjectResult(result.Value),
            { IsSuccess: true, Value: null, Message: not null } => new OkObjectResult(result.Message),
            { IsSuccess: true, Value: null, StatusCode: (int)HttpStatusCode.NoContent } => new NoContentResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }

    protected IActionResult HandlePagedResult<T>(Result<PaginatedList<T>> result) where T : class
    {
        switch (result)
        {
            case { IsSuccess: false, Message: not null }:
                return new BadRequestObjectResult(result.Message);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: > 0 } }:
                Response.AddPaginationHeader(result.Value.PageNumber, result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
                return new OkObjectResult(result.Value);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: 0 } }:
                return new NoContentResult();
            case { IsSuccess: true, Value: null }:
                return new NoContentResult();
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}