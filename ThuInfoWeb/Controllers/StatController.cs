﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThuInfoWeb.DBModels;

namespace ThuInfoWeb.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class StatController(Data data) : ControllerBase
{
    [Route("{function:int}")]
    public async Task<IActionResult> Usage(int function)
    {
        if (!Enum.IsDefined(typeof(Usage.FunctionType), function))
            return BadRequest("功能不存在");
        var usage = new Usage { Function = (Usage.FunctionType)function, CreatedTime = DateTime.Now };
        var result = await data.CreateUsageAsync(usage);
        if (result != 1)
            return BadRequest();
        return Ok();
    }

    [Route("")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UsageData()
    {
        return Ok(await data.GetUsageAsync());
    }

    public async Task<IActionResult> Startup()
    {
        var s = new Startup { CreatedTime = DateTime.Now };
        var result = await data.CreateStartupAsync(s);
        if (result != 1)
            return BadRequest();
        return Ok();
    }

    [Route("")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> StartupData()
    {
        return Ok(await data.GetStartupDataAsync());
    }
#if DEBUG
    public async Task<IActionResult> GenStartupData()
    {
        await data.GenStartupDataAsync();
        return Ok();
    }
#endif
}
