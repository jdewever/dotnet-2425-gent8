namespace Rise.Shared.Products;

public interface IProductService
{
    Task<ProductResponse> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<string>> GetAllLocations();
    Task<ProductDTO> GetProductByBarcode(string? barcode = null);
    Task<IEnumerable<ProductDTO>> GetProductsHavingLowStock();
}
