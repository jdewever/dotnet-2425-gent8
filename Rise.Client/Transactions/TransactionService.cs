using System.Net.Http.Json;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;

namespace Rise.Client.Transactions;

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task AddTransactionScanOut(List<CartItem> cartItems)
    {
        throw new NotImplementedException();
    }

    public Task AddTransactionScanIn(List<CartItem> cartItems)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TransactionDTO>> GetRecentTransactions()
    {
        var response = await _httpClient.GetFromJsonAsync<List<TransactionDTO>>("history/recent/transactions");
        return response ?? [];
    }
}