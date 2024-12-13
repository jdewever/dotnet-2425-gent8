using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Services.Auth;
using Rise.Shared.User;
using Serilog;
using System;

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
    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        Log.Information("Getting all users✨");
        return await userService.GetUsers();
    }

    [HttpGet("details")]
    public async Task<UserDTO> GetUser()
    {
        var userid = _authContextProvider.User?.Identity?.Name;
        if (userid is null)
            throw new ArgumentNullException("User not found");

        Log.Information("Getting user using id:{userid}✨", userid);
        return await userService.GetUserCache(userid);
    }

    [HttpPost("{userId}/block")]
    [Authorize(Roles = "Administrator")]
    public async Task<UserDTO> BlockUser(string userId)
    {
        return await userService.BlockUser(userId);
    }
}