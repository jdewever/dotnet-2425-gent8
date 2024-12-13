using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Domain.DomainClasses;
using Rise.Shared.Booking;
using Rise.Shared.Products;
using Rise.Shared.Transaction;
using Serilog;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoryController
{
    private readonly ITransactionService _transactionService;
    private readonly IBookingService _bookingService;

    public HistoryController(ITransactionService transactionService, IBookingService bookingService)
    {
        _transactionService = transactionService;
        _bookingService = bookingService;
    }

    [HttpGet("recent/transactions")]
    public async Task<List<TransactionDTO>> GetRecentTransactionHistory()
    {
        Log.Information("Getting transactions history ✨");
        return await _transactionService.GetRecentTransactions();
    }

    [HttpGet("recent/bookings")]
    public async Task<IEnumerable<BookingDTO>> GetRecentBookingHistory([FromQuery] bool history)
    {
        Log.Information("Getting bookings history ✨");
        return await _bookingService.GetRecentBookings(history);
    }
}