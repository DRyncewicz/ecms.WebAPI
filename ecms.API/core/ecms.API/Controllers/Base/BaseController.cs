using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecms.API.Controllers.Base;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
}