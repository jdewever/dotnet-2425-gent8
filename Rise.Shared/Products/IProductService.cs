using System.Threading;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<IEnumerable<ProductDto>> GetSearchedProducts(string? searchTerm = null);
}