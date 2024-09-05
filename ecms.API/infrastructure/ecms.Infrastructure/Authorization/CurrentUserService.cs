using ecms.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;
using SharedKernal;
using System.Security.Claims;

namespace ecms.Infrastructure.Authorization;

public class CurrentUserService : ICurrentUserService
{
    public string UserId { get; set; }

    public bool IsAuthenticated { get; set; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var userid = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        Ensure.NotNullOrEmpty(userid);

        UserId = userid;

        IsAuthenticated = true;
    }
}