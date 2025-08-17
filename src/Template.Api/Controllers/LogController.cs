using Microsoft.AspNetCore.Mvc;
using Template.Infrastructure.Logging;

namespace Template.Api.Controllers;

[Route("api/[controller]")]
public class LogController : BaseController
{
    private readonly ILogManager<LogController> _logManager;

    public LogController(ILogManager<LogController> logManager)
    {
        _logManager = logManager;
    }

    [HttpGet("TestLogging")]
    public IActionResult TestLogging()
    {
        _logManager.Info("Info seviyesi test logu", payload: new { User = "User", Action = "TestLogging" });
        _logManager.Warn("Warn seviyesi test logu");
        _logManager.Error("Error seviyesi test logu", new Exception("Exception"));
        _logManager.Fatal("Fatal seviyesi test logu");

        return Ok();
    }
}
