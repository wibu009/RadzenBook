using RadzenBook.Application.Catalog.Customer;

namespace RadzenBook.Host.Controllers;

[ApiVersion(ApiVersionName.V2)]
[Authorize(Roles = RoleName.Manager)]
public class CustomerController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get Customers")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged Customers", typeof(PaginatedList<CustomerDto>))]
    public async Task<IActionResult> GetPagedCustomers(
        [FromQuery] PagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetCustomerListRequest { PagingParams = pagingParams },
            cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get Customer by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get Customer by id", typeof(CustomerDto))]
    public async Task<IActionResult> GetCustomerById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetCustomerByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Create Customer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create Customer successfully")]
    public async Task<IActionResult> CreateCustomer(
        [FromBody] CreateCustomerRequest createCustomerRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createCustomerRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update Customer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update Customer successfully")]
    public async Task<IActionResult> UpdateCustomer(
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerRequest updateCustomerRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(
            updateCustomerRequest.SetPropertyValue(nameof(UpdateCustomerRequest.Id), id), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete Customer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete Customer successfully")]
    public async Task<IActionResult> DeleteCustomer(
        [SwaggerParameter("Customer id")] [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteCustomerRequest { Id = id }, cancellationToken));
}