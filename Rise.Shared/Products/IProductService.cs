using System.Threading;

namespace Rise.Shared.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<string>> GetAllLocations();
    Task<ProductDTO> GetProductByBarcode(string? barcode = null);
}