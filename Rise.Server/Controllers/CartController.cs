using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;
using Rise.Domain.DomainClasses;
using Serilog;

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
        foreach (var item in cartItems)
        {
            Log.Information("Checking out items: {ProductName} with Quantity: {Quantity} ✨", item.Product.Name, item.Quantity);
        }

        await transactionService.AddTransactionScanOut(cartItems);
        await cartService.CheckoutItems(cartItems);
        return Ok();
    }


    [HttpPut("checkin")]
    public async Task<IActionResult> PutCheckIn([FromBody] List<CartItem> cartItems)
    {
        foreach (var item in cartItems)
        {
            Log.Information("Checking in items: {ProductName} with Quantity: {Quantity} ✨", item.Product.Name, item.Quantity);
        }

        await transactionService.AddTransactionScanIn(cartItems);
        await cartService.CheckInItems(cartItems);
        return Ok();
    }

}