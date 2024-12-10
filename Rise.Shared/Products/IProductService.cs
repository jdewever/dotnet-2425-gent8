namespace Rise.Shared.Products;

public interface IProductService
{
    Task<ProductResponse> GetAllProducts(ProductRequest.Index request);
    Task<IEnumerable<string>> GetAllLocations();
    Task<ProductDTO> GetProductByBarcode(string barcode);
    Task<DashboardDTO> GetDashboardInfo();
    Task AddProduct(ProductCreationDTO product);
    Task ToggleHideProduct(string barcode);
    Task DeleteProduct(string barcode);
    Task UpdateProduct(string barcode, ProductCreationDTO product);
    Task<List<ProductDTO>> GetHiddenProducts(ProductRequest.Hidden request);
    Task<string> UploadImage(Stream fileStream, string contentType);
}
