using System.Security.Claims;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.User;

namespace Rise.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly IManagementApiClient _managementApiClient;

    public UserController(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
        return users.Select(x => new UserDto
        {
            Email = x.Email,
            //zit niet in auth0
            // FirstName = x.FirstName,
            // LastName = x.LastName,
            IsBlocked = x.Blocked ?? false,
            Fullname = x.FullName,
            Picture = x.Picture,
        });
    }

    [HttpGet("details")]
    public async Task<UserDto> GetUser()
    {
        var userid = User.Claims.FirstOrDefault(x => x.Type ==  ClaimTypes.NameIdentifier)?.Value;
        var user = await _managementApiClient.Users.GetAsync(userid);
        return new UserDto
        {
            Email = user.Email,
            // zit niet in auth0
            // FirstName = user.FirstName ?? "",
            // LastName = user.LastName ?? "",
            IsBlocked = user.Blocked ?? false,
            Picture = user.Picture,
            Fullname = user.FullName
        };
    }
}