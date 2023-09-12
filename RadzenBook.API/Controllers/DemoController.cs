using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO.Demo;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.API.Controllers;

public class DemoController : BaseApiController
{
    [HttpGet]
    [Authorize(Roles = "manager")]
    [SwaggerOperation(Summary = "Get all demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get all demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetDemos() 
        => HandlePagedResult(await FeatureServiceManager.DemoService.GetAllDemosAsync());

    [HttpGet("paged")]
    [SwaggerOperation(Summary = "Get paged demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetPagedDemos([FromQuery] PagingParams pagingParams) 
        => HandlePagedResult(await FeatureServiceManager.DemoService.GetPagedDemosAsync(pagingParams));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get demo by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get demo by id", typeof(DemoDto))]
    public async Task<IActionResult> GetDemoById([FromRoute] Guid id) 
        => HandleResult(await FeatureServiceManager.DemoService.GetDemoByIdAsync(id));

    [HttpPost]
    [SwaggerOperation(Summary = "Create demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create demo successfully")]
    public async Task<IActionResult> CreateDemo(
        [FromBody]
        [SwaggerParameter("Demo create dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        DemoCreateDto demoCreateDto)
        => HandleResult(await FeatureServiceManager.DemoService.CreateDemoAsync(demoCreateDto));

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update demo successfully")]
    public async Task<IActionResult> UpdateDemo(
        [FromRoute]
        [SwaggerParameter("Demo id")]
        Guid id,
        [FromBody]
        [SwaggerParameter("Demo update dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        DemoUpdateDto demoUpdateDto)
        => HandleResult(await FeatureServiceManager.DemoService.UpdateDemoAsync(id, demoUpdateDto));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete demo successfully")]
    public async Task<IActionResult> DeleteDemo(
        [FromRoute]
        [SwaggerParameter("Demo id")]
        Guid id)
        => HandleResult(await FeatureServiceManager.DemoService.DeleteDemoAsync(id));
}