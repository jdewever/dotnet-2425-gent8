using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeProductService : IProductService
{
    public Task<IEnumerable<ProductDto>> GetAllProducts(ProductRequest.Index request)
    {
        var products = Enumerable.Range(1, 5)
                                 .Select(i => new ProductDto { Id = i, Name = $"Product {i}",Barcode = $"Barcode {i}",Description = $"Description {i}", ClassRoomCode = $"ClassRoom {i}", QuantityInStock = i, QuantityOnOrder = i,Categories = null});

        return Task.FromResult(products);
    }
}

