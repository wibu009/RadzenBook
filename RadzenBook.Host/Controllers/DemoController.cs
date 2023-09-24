using RadzenBook.Application.Catalog.Demo.Command;
using RadzenBook.Application.Catalog.Demo.Query;

namespace RadzenBook.Host.Controllers;

[Authorize(Roles = "manager")]
public class DemoController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetPagedDemos(
        [FromQuery]
        PagingParams pagingParams,
        CancellationToken cancellationToken)
        => HandlePagedResult(await Mediator.Send(new GetDemoRequest { PagingParams = pagingParams }, cancellationToken));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get demo by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get demo by id", typeof(DemoDto))]
    public async Task<IActionResult> GetDemoById(
        [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new GetDemoByIdRequest { Id = id }, cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Create demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create demo successfully")]
    public async Task<IActionResult> CreateDemo(
        [FromBody]
        CreateDemoRequest createDemoRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(createDemoRequest, cancellationToken));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update demo successfully")]
    public async Task<IActionResult> UpdateDemo(
        [FromRoute]
        Guid id,
        [FromBody]
        UpdateDemoRequest updateDemoRequest,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(UpdateDemoRequest.SetId(id, updateDemoRequest), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete demo successfully")]
    public async Task<IActionResult> DeleteDemo(
        [SwaggerParameter("Demo id")]
        [FromRoute]
        Guid id,
        CancellationToken cancellationToken)
        => HandleResult(await Mediator.Send(new DeleteDemoRequest { Id = id }, cancellationToken));
}