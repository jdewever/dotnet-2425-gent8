using System.Threading;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<ProductDto>> GetSearchedProducts(string? searchTerm = null);
}