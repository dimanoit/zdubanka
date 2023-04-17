using Api.Extensions;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[SocialAuthFilter]
public class TestController: ControllerBase
{
    private readonly AppSettings _appSettings;

    public TestController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public IActionResult Get()
    {
        var user = HttpContext.GetCurrentAccountAsync(_appSettings);
        return Ok();
    }
}