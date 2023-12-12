using RadzenBook.Application.Catalog.Category;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
public class CategoryController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get categories")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged categories", typeof(PaginatedList<CategoryDto>))]
    public async Task<IActionResult> GetPagedCategories(
        [FromQuery] CategoryPagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetCategoryListRequest { PagingParams = pagingParams },
            cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get category by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get category by id", typeof(CategoryDto))]
    public async Task<IActionResult> GetCategoryById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetCategoryByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Create category")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create category successfully")]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest createCategoryRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createCategoryRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update category")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update category successfully")]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest updateCategoryRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateCategoryRequest.SetPropertyValue(nameof(UpdateCategoryRequest.Id), id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete category")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete category successfully")]
    public async Task<IActionResult> DeleteCategory(
        [SwaggerParameter("Category id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteCategoryRequest { Id = id }, cancellationToken));
}