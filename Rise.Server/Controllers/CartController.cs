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
    private readonly ITransactionItemService transactionItemService;

    public CartController(ICartService cartService, ITransactionService transactionService, ITransactionItemService transactionItemService)
    {
        this.cartService = cartService;
        this.transactionService = transactionService;
        this.transactionItemService = transactionItemService;
    }

    [HttpPut("checkout")]
    public async Task<IActionResult> Put([FromBody] List<CartItem> cartItems)
    {
        var transactionID = await transactionService.AddTransactionScanOut();
        await transactionItemService.AddTransactionItems(transactionID, cartItems);
        await cartService.CheckoutItems(cartItems);
        return Ok();
    }

    [HttpPut("checkin")]
    public async Task<IActionResult> PutCheckIn([FromBody] List<CartItem> cartItems)
    {
        var transactionID = await transactionService.AddTransactionScanIn();
        await transactionItemService.AddTransactionItems(transactionID, cartItems);
        await cartService.CheckInItems(cartItems);
        return Ok();
    }

}