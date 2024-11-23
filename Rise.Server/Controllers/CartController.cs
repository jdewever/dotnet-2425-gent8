using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService cartService;
    private readonly ITransactionService transactionService;

    public CartController(ICartService cartService, ITransactionService transactionService)
    {
        this.cartService = cartService;
        this.transactionService = transactionService;
    }

    [HttpPut("checkout")]
    public async Task<IActionResult> Put([FromBody] List<CartItem> cartItems)
    {
        await transactionService.AddTransactionScanOut(cartItems);
        await cartService.CheckoutItems(cartItems);
        return Ok();
    }

    [HttpPut("checkin")]
    public async Task<IActionResult> PutCheckIn([FromBody] List<CartItem> cartItems)
    {
        await transactionService.AddTransactionScanIn(cartItems);
        await cartService.CheckInItems(cartItems);
        return Ok();
    }

}