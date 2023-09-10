using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO.Demo;
using RadzenBook.Service.Interfaces.Features;
using Swashbuckle.AspNetCore.Annotations;

namespace RadzenBook.API.Controllers;

public class DemoController : BaseApiController
{
    private readonly IDemoService _demoService;

    public DemoController(IFeaturesServiceManager featuresServiceManager)
    {
        _demoService = featuresServiceManager.DemoService;
    }

    [HttpGet]
    [Authorize(Roles = "manager")]
    [SwaggerOperation(Summary = "Get all demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get all demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetDemos()
    {
        return HandlePagedResult(await _demoService.GetAllDemosAsync());
    }

    [HttpGet("paged")]
    [SwaggerOperation(Summary = "Get paged demos")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get paged demos", typeof(PaginatedList<DemoDto>))]
    public async Task<IActionResult> GetPagedDemos([FromQuery] PagingParams pagingParams)
    {
        return HandlePagedResult(await _demoService.GetPagedDemosAsync(pagingParams));
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get demo by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get demo by id", typeof(DemoDto))]
    public async Task<IActionResult> GetDemoById([FromRoute] Guid id)
    {
        return HandleResult(await _demoService.GetDemoByIdAsync(id));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Create demo successfully", typeof(string))]
    public async Task<IActionResult> CreateDemo(
        [FromBody]
        [SwaggerParameter("Demo create dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        DemoCreateDto demoCreateDto)
    {
        return HandleResult(await _demoService.CreateDemoAsync(demoCreateDto));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Update demo successfully", typeof(string))]
    public async Task<IActionResult> UpdateDemo(
        [FromRoute]
        [SwaggerParameter("Demo id")]
        Guid id,
        [FromBody]
        [SwaggerParameter("Demo update dto (DemoEnum: Demo1, Demo2, Demo3, Demo 4)")]
        DemoUpdateDto demoUpdateDto)
    {
        return HandleResult(await _demoService.UpdateDemoAsync(id, demoUpdateDto));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete demo")]
    [SwaggerResponse(StatusCodes.Status200OK, "Delete demo successfully", typeof(string))]
    public async Task<IActionResult> DeleteDemo(
        [FromRoute]
        [SwaggerParameter("Demo id")]
        Guid id)
    {
        return HandleResult(await _demoService.DeleteDemoAsync(id));
    }
}