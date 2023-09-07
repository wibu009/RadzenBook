using System.Net;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Api.Extensions;
using RadzenBook.Contract.Core;

namespace RadzenBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result) where T : class
    {
        return result switch
        {
            { IsSuccess: false, Error: not null } => new BadRequestObjectResult(result.Error),
            { IsSuccess: true, Value: not null } => new OkObjectResult(result.Value),
            { IsSuccess: true, Value: null } => new NoContentResult(),
            _ => new StatusCodeResult((int)HttpStatusCode.InternalServerError)
        };
    }

    protected IActionResult HandlePagedResult<T>(Result<PaginatedList<T>> result) where T : class
    {
        switch (result)
        {
            case { IsSuccess: false, Error: not null }:
                return new BadRequestObjectResult(result.Error);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: > 0 } }:
                Response.AddPaginationHeader(result.Value.PageNumber, result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
                return new OkObjectResult(result.Value);
            case { IsSuccess: true, Value: not null, Value: { TotalCount: 0 } }:
                return new NoContentResult();
            case { IsSuccess: true, Value: null }:
                return new NoContentResult();
            default:
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}