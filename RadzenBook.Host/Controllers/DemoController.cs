using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Application.Catalog.Demo;
using RadzenBook.Application.Catalog.Demo.Command;
using RadzenBook.Application.Catalog.Demo.Query;
using RadzenBook.Application.Common.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.Host.Controllers;

[Authorize(Roles = "manager")]
public class DemoController : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get all demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetAllDemos() 
        => HandlePagedResult(await Mediator.Send(new GetAllDemoRequest()));

    [HttpGet("paged")]
    [SwaggerOperation(Summary = "Get paged demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetPagedDemos([FromQuery] PagingParams pagingParams)
        => HandlePagedResult(await Mediator.Send(new GetPagedDemoRequest { PagingParams = pagingParams }));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get demo by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get demo by id", typeof(DemoDto))]
    public async Task<IActionResult> GetDemoById([FromRoute] Guid id) 
        => HandleResult(await Mediator.Send(new GetDemoByIdRequest { Id = id }));

    [HttpPost]
    [SwaggerOperation(Summary = "Create demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create demo successfully")]
    public async Task<IActionResult> CreateDemo(
        [FromBody]
        [SwaggerParameter("Demo create dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        CreateDemoRequest createDemoRequest)
        => HandleResult(await Mediator.Send(createDemoRequest));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update demo successfully")]
    public async Task<IActionResult> UpdateDemo(
        [FromRoute] Guid id,
        [FromBody]
        [SwaggerParameter("Demo update dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        UpdateDemoRequest updateDemoRequest)
        => HandleResult(await Mediator.Send(UpdateDemoRequest.SetId(id, updateDemoRequest)));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete demo successfully")]
    public async Task<IActionResult> DeleteDemo(
        [SwaggerParameter("Demo id")]
        [FromRoute] Guid id)
        => HandleResult(await Mediator.Send(new DeleteDemoRequest { Id = id }));
}