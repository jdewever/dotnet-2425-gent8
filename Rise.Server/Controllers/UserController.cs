using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Services.Auth;
using Rise.Shared.User;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IAuthContextProvider _authContextProvider;

    public UserController(IUserService userService, IAuthContextProvider authContextProvider)
    {
        if (authContextProvider.User is null)
            throw new ArgumentNullException($"{nameof(UserController)} requires a {nameof(authContextProvider)}");
        this.userService = userService;
        _authContextProvider = authContextProvider;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return await userService.GetUsers();
    }

    [HttpGet("details")]
    public async Task<UserDto> GetUser()
    {
        var userid = _authContextProvider.User?.Identity?.Name;
        if (userid is null)
            throw new ArgumentNullException("User not found");

        return await userService.GetUser(userid);
    }
}