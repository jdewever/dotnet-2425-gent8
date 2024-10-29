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

    public Task<IEnumerable<string>> GetAllLocations()
    {
        var locations = Enumerable.Range(1, 5)
            .Select(i => $"Location {i}");
        return Task.FromResult(locations);
    }

    public Task<ProductDTO> GetProductByBarcode(string? barcode = null)
    {
        var products = GetAllProducts(new ProductRequest.Index()).Result;
        return Task.FromResult(products.Where(p => p.Barcode == barcode).First());
    }
}

