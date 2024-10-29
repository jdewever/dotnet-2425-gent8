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
        var product = new ProductDTO { Id = 1, Name = "Product 1", Barcode = "Barcode 1", Description = "Description 1", ClassRoomCode = "ClassRoom 1", QuantityInStock = 1, QuantityOnOrder = 1, Categories = null };
        return Task.FromResult(product);
    }
}

