using System.Diagnostics;
using System.Net.Http.Json;
using Rise.Shared.Cart;
using Rise.Shared.Transaction;

namespace Rise.Client.Profile;

public class TransactionService
{
    private readonly HttpClient _httpClient;

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TransactionDto.History>> GetRecentTransactions()
    {
        var response = await _httpClient.GetFromJsonAsync<List<TransactionDto.History>>("history/recent/transactions");
        return response ?? [];
    }
}