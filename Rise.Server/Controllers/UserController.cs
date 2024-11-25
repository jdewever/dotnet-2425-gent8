using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Services.Auth;
using Rise.Shared.User;

namespace Rise.Server.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly IManagementApiClient _managementApiClient;
    private readonly IAuthContextProvider _authContextProvider;

    public UserController(IManagementApiClient managementApiClient, IAuthContextProvider authContextProvider)
    {
        if (authContextProvider.User is null)
            throw new ArgumentNullException($"{nameof(UserController)} requires a {nameof(authContextProvider)}");
        _managementApiClient = managementApiClient;
        _authContextProvider = authContextProvider;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
        return users.Select(x => new UserDto
        {
            Email = x.Email,
            FullName = x.FullName,
            Picture = x.Picture,
            IsBlocked = x.Blocked ?? false,
        });
    }
    [HttpGet("details")]
    public async Task<UserDto> GetUser()
    {
        var userid = _authContextProvider.User?.Identity?.Name;
        var user = await _managementApiClient.Users.GetAsync(userid);
        return new UserDto
        {
            Email = user.Email,
            IsBlocked = user.Blocked ?? false,
            Picture = user.Picture,
            FullName = user.FullName
        };
    }
}