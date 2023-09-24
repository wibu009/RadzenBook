using RadzenBook.Application.Catalog.Address.Query;

namespace RadzenBook.Host.Controllers;

public class AddressController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get addresses")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get addresses", typeof(PaginatedList<AddressDto>))]
    public async Task<IActionResult> GetAddresses(
        [FromQuery]
        PagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetAddressRequest { PagingParams = pagingParams }, cancellationToken));


    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get address by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get address by id", typeof(AddressDto))]
    public async Task<IActionResult> GetAddressById(
        [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetAddressByIdRequest { Id = id }, cancellationToken));
}