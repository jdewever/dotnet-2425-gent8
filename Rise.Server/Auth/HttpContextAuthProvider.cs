using System.Security.Claims;
using Rise.Services.Auth;

namespace Rise.Server.Auth;

public class HttpContextAuthProvider(IHttpContextAccessor httpContextAccessor) : IAuthContextProvider
{
    public ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
}