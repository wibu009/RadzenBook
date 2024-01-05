using RadzenBook.Application.Catalog.Publisher;


namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
public class PublisherController : BaseApiController
{

    [HttpGet]
    [SwaggerOperation(Summary = "Get Publishes")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged Publishes", typeof(PaginatedList<PublisherDto>))]
    public async Task<IActionResult> GetPagedPublishes(
       [FromQuery] PagingParams pagingParams,
       CancellationToken cancellationToken)
       => HandlePagedResult(await Mediator.Send(new GetPublisherListRequest { PagingParams = pagingParams }, cancellationToken));


    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get Publisher by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get Publisher by id", typeof(PublisherDto))]
    public async Task<IActionResult> GetPublisherById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetPublisherByIdRequest { Id = id }, cancellationToken));


    //ThemMoi

    [HttpPost]
    [SwaggerOperation(Summary = "Create Publishers")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create Publishers successfully")]
    public async Task<IActionResult> CreatePublisher(
       [FromBody] CreatePublisherRequest  createPublisherRequest, 
       CancellationToken cancellationToken)
       => HandleResult(await Mediator.Send(createPublisherRequest, cancellationToken));





    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update Publisher")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update Publisher successfully")]
    public async Task<IActionResult> UpdatePublisher(
        [FromRoute] Guid id,
        [FromBody] UpdatePublisherRequest updatePublisherRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
           updatePublisherRequest.SetPropertyValue(nameof(UpdatePublisherRequest.Id), id), cancellationToken));



    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete Publisher")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete Publisher successfully")]
    public async Task<IActionResult> DeletePublisher(
        [SwaggerParameter("Publisher id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeletePublisherRequest { Id = id }, cancellationToken));


}

