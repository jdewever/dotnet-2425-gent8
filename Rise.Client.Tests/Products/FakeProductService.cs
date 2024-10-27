using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeProductService : IProductService
{
    public Task<IEnumerable<ProductDTO>> GetAllProducts(ProductRequest.Index request)
    {
        var products = Enumerable.Range(1, 5)
                                 .Select(i => new ProductDTO { Id = i, Name = $"Product {i}",Barcode = $"Barcode {i}",Description = $"Description {i}", ClassRoomCode = $"ClassRoom {i}", QuantityInStock = i, QuantityOnOrder = i,Categories = null});

        return Task.FromResult(products);
    }

    public Task<ProductDTO> GetProductByBarcode(string? barcode = null)
    {
        var products = GetAllProducts(new ProductRequest.Index()).Result;
        return Task.FromResult(products.Where(p => p.Barcode == barcode).First());
    }

    public Task<IEnumerable<ProductDTO>> GetSearchedProducts(string? searchTerm = null)
    {
        var products = GetAllProducts(new ProductRequest.Index()).Result;
        var term = searchTerm?.ToLower() ?? string.Empty;
        return Task.FromResult(products.Where(p => p.Name.ToLower().Contains(term) || p.Barcode.ToLower().Contains(term)));
    }
}

