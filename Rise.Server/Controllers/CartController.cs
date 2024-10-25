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
    public async Task Put([FromBody] Dictionary<ProductDTO, int> cart)
    {
        // TODO
        /* var result = */
        Console.WriteLine("Test");
        //await cartService.CheckoutItems(cart);
        //return result;
    }

}