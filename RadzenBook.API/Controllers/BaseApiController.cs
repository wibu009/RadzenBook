using FirstBlazorProject_BookStore.API.Extensions;
using FirstBlazorProject_BookStore.Model.Core;
using FirstBlazorProject_BookStore.Model.Cores;
using Microsoft.AspNetCore.Mvc;

namespace FirstBlazorProject_BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult PaginatedResponse<TDto>(PaginatedList<TDto> paginatedList) where TDto : class
    {
        Response.AddPaginationHeader(paginatedList.PageNumber, paginatedList.PageSize, paginatedList.Count, paginatedList.TotalPages);
        return new ObjectResult(paginatedList) { StatusCode = StatusCodes.Status200OK };
    }
    
    protected IActionResult HandleResult<TDto>(Result<TDto> result) where TDto : class
    {
        return result.IsSuccess switch
        {
            true when result.Value != null => Ok(result.Value),
            true when result.Value == null => NoContent(),
            false when result.Error != null => BadRequest(result.Error),
            _ => BadRequest()
        };
    }
}