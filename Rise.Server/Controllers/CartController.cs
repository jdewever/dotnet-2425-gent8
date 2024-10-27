using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Cart;
using Rise.Shared.Products;

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
    public async Task<IActionResult> Put([FromBody] List<CartItem> cartItems)
    {
        // TODO
        /* var result = */
        //await cartService.CheckoutItems(cart);
        //return result;
        var cart = new Dictionary<ProductDTO, int>();
        foreach (var item in cartItems)
        {
            cart[item.Product] = item.Quantity;
        }

        await cartService.CheckoutItems(cart);
        return Ok();
    }

}