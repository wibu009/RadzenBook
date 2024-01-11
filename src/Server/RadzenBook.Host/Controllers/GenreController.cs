using RadzenBook.Application.Catalog.Genre;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
public class GenreController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get genres")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged genres", typeof(PaginatedList<GenreDto>))]
    public async Task<IActionResult> GetPagedGenres(
        [FromQuery] GenrePagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetGenreListRequest { PagingParams = pagingParams },
            cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get genre by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get genre by id", typeof(GenreDto))]
    public async Task<IActionResult> GetGenreById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetGenreByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Create genre")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create genre successfully")]
    public async Task<IActionResult> CreateGenre(
        [FromBody] CreateGenreRequest createGenreRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createGenreRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update genre")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update genre successfully")]
    public async Task<IActionResult> UpdateGenre(
        [FromRoute] Guid id,
        [FromBody] UpdateGenreRequest updateGenreRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateGenreRequest.SetPropertyValue(nameof(UpdateGenreRequest.Id), id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete genre")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete genre successfully")]
    public async Task<IActionResult> DeleteGenre(
        [SwaggerParameter("Genre id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteGenreRequest { Id = id }, cancellationToken));
}