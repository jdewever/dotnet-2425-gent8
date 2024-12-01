using Rise.Shared.Products;

namespace Rise.Shared.Transaction;

public class TransactionItemDTO
{
    public required ProductDTO Product { get; set; }
    public required int Quantity { get; set; }
}