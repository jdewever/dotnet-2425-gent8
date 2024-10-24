using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Cart;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService cartService;

    public CartController(ICartService cartService)
    {
        this.cartService = cartService;
    }

    [HttpPut]
    public async Task Put([FromBody] CartDTO cart)
    {
        // TODO
        /* var result = */
        await cartService.CheckoutItems(cart);
        //return result;
    }

}