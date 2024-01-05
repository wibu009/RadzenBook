
using RadzenBook.Application.Catalog.PublisherAddress;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
public class PublisherAddressController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get PublisherAddress")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged PublisherAddress", typeof(PaginatedList<PublisherAddressDto>))]
    public async Task<IActionResult> GetPagedPublisherAddress(
        [FromQuery] PagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetPublisherAddressListRequest { PagingParams = pagingParams },
            cancellationToken));


    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get PublisherAddresss by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get PublisherAddress by id", typeof(PublisherAddressDto))]
    public async Task<IActionResult> GetCustomerAddressById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetPublisherAddressByIdRequest { Id = id }, cancellationToken));




    [HttpPost]
    [SwaggerOperation(Summary = "Create PublisherAddresss")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create PublisherAddresss successfully")]
    public async Task<IActionResult> CreatePublisherAddresss(
       [FromBody] CreatePublisherAddressRequest createCustomerAddressRequest,
       CancellationToken cancellationToken)
       => HandleResult(await Mediator.Send(createCustomerAddressRequest, cancellationToken));





    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update PublisherAddresss")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update PublisherAddresss successfully")]
    public async Task<IActionResult> UpdateCustomerAddress(
        [FromRoute] Guid id,
        [FromBody] UpdatePublisherAddressRequest updateCustomerAddressRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateCustomerAddressRequest.SetPropertyValue(nameof(UpdatePublisherAddressRequest.Id), id), cancellationToken));


    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete PublisherAddresss")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete PublisherAddresss successfully")]
    public async Task<IActionResult> DeletePublisherAddresss(
     [SwaggerParameter("PublisherAddresss id")] [FromRoute]
        Guid id,
     CancellationToken cancellationToken)
     => HandleResult(await Mediator.Send(new DeletePublisherAddressRequest { Id = id }, cancellationToken));
}

