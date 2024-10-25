using System.Threading;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<ProductResponse> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<string>> GetAllLocations();
}
