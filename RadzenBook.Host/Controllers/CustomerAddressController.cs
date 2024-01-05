using RadzenBook.Application.Catalog.CustomerAddress;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
[Authorize(Roles = RoleName.Manager)]
public class CustomerAddressController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get CustomerAddresss")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged CustomerAddresss", typeof(PaginatedList<CustomerAddressDto>))]
    public async Task<IActionResult> GetPagedCustomerAddress(
        [FromQuery] PagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetCustomerAddressListRequest { PagingParams = pagingParams },
            cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get CustomerAddress by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get CustomerAddress by id", typeof(CustomerAddressDto))]
    public async Task<IActionResult> GetCustomerAddressById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetCustomerAddressByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Create CustomerAddress")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create CustomerAddress successfully")]
    public async Task<IActionResult> CreateCustomerAddress(
        [FromBody] CreateCustomerAddressRequest createCustomerAddressRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createCustomerAddressRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update CustomerAddress")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update CustomerAddress successfully")]
    public async Task<IActionResult> UpdateCustomerAddress(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerAddressRequest updateCustomerAddressRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateCustomerAddressRequest.SetPropertyValue(nameof(UpdateCustomerAddressRequest.Id), id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete CustomerAddress")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete CustomerAddress successfully")]
    public async Task<IActionResult> DeleteCustomerAddress(
        [SwaggerParameter("CustomerAddress id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteCustomerAddressRequest { Id = id }, cancellationToken));
}