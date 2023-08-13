using FirstBlazorProject_BookStore.API.Extensions;
using FirstBlazorProject_BookStore.Common.Core;
using Microsoft.AspNetCore.Mvc;

namespace FirstBlazorProject_BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult PaginatedResponse<TDto>(PaginatedResult<TDto> paginatedResult) where TDto : class
    {
        Response.AddPaginationHeader(paginatedResult.PageNumber, paginatedResult.PageSize, paginatedResult.Count, paginatedResult.TotalPages);
        return new ObjectResult(paginatedResult) { StatusCode = StatusCodes.Status200OK };
    }
}