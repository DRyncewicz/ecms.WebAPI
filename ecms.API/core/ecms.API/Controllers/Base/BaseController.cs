using Microsoft.AspNetCore.Mvc;

namespace ecms.API.Controllers.Base;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
}