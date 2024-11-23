using System.Security.Claims;

namespace Rise.Services.Auth;

public interface IAuthContextProvider
{
    ClaimsPrincipal? User { get; }
}