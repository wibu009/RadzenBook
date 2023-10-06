using MediatR;

namespace RadzenBook.Host.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    private IInfrastructureServiceManager? _infrastructureServiceManager;
    protected IMediator Mediator => _mediator ??= 
        (HttpContext.RequestServices.GetService<IMediator>() ?? throw new NullReferenceException(nameof(IMediator)));
    protected IInfrastructureServiceManager InfrastructureServiceManager => _infrastructureServiceManager ??= 
        (HttpContext.RequestServices.GetService<IInfrastructureServiceManager>() ?? throw new NullReferenceException(nameof(IInfrastructureServiceManager)));
    
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result switch
        {
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.BadRequest } 
                => BadRequest(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Unauthorized }
                => Unauthorized(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Forbidden } 
                => Forbid(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.NotFound } 
                => NotFound(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.Conflict } 
                => Conflict(result.Message),
            { IsSuccess: false, Message: not null, StatusCode: (int)HttpStatusCode.InternalServerError } 
                => StatusCode((int)HttpStatusCode.InternalServerError, result.Message),
            { IsSuccess: false, Message: not null } 
                => BadRequest(result.Message),
            { IsSuccess: true, Value: not null, StatusCode: (int)HttpStatusCode.Redirect }
                => Request.Headers["Referer"].ToString().Contains("/swagger") 
                    ? Ok($"Redirection to {result.Value.ToString()}")
                    : Redirect(result.Value.ToString()!),
            { IsSuccess: true, Value: not null } 
                => Ok(result.Value),
            { IsSuccess: true, Value: null, StatusCode: (int)HttpStatusCode.NoContent } 
                => NoContent(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }

    protected IActionResult HandlePagedResult<T>(Result<PaginatedList<T>> result) where T : class
    {
        switch (result)
        {
            case { IsSuccess: false, Message: not null }:
                return BadRequest(result.Message);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: > 0 } }:
                Response.AddPaginationHeader(result.Value.PageNumber, result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
                return Ok(result.Value);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: 0 } }:
                return NoContent();
            case { IsSuccess: true, Value: null }:
                return NotFound();
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}