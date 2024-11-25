using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Transaction;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HistoryController
{
    private readonly ITransactionService _transactionService;

    public HistoryController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpGet("recent/transactions")]
    public async Task<List<TransactionDto.History>> GetRecentTransactionHistory()
    {
        return await _transactionService.GetRecentTransactions();
    }
}