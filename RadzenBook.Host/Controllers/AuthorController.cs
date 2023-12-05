using RadzenBook.Application.Catalog.Author;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
public class AuthorController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get authors")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged authors", typeof(PaginatedList<AuthorDto>))]
    public async Task<IActionResult> GetPagedAuthors(
        [FromQuery] AuthorPagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetAuthorListRequest { PagingParams = pagingParams },
            cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get author by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get author by id", typeof(AuthorDto))]
    public async Task<IActionResult> GetAuthorById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetAuthorByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [Authorize(Roles = RoleName.Manager)]
    [SwaggerOperation(Summary = "Create author")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create author successfully")]
    public async Task<IActionResult> CreateAuthor(
        [FromForm] CreateAuthorRequest createAuthorRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createAuthorRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RoleName.Manager)]
    [SwaggerOperation(Summary = "Update author")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update author successfully")]
    public async Task<IActionResult> UpdateAuthor(
        [FromRoute] Guid id,
        [FromForm] UpdateAuthorRequest updateAuthorRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateAuthorRequest.SetPropertyValue(nameof(UpdateAuthorRequest.Id), id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RoleName.Manager)]
    [SwaggerOperation(Summary = "Delete author")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete author successfully")]
    public async Task<IActionResult> DeleteAuthor(
        [SwaggerParameter("Author id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteAuthorRequest { Id = id }, cancellationToken));
}