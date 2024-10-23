using System.Threading;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<ProductDTO>> GetSearchedProducts(string? searchTerm = null);
    Task<IEnumerable<string>> GetAllLocations();
}