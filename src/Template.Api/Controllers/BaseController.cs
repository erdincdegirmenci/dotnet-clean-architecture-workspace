using Microsoft.AspNetCore.Mvc;
using Template.Api.Middlewares;

namespace Template.Api.Controllers;

[ApiController]
[ServiceFilter(typeof(CustomExceptionFilter))]
public class BaseController : ControllerBase
{
}

