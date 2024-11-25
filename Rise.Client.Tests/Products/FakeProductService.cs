using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public class FakeProductService : IProductService
{
    public Task<ProductResponse> GetAllProducts(ProductRequest.Index request)
    {
        var products = Enumerable.Range(1, 5)
                                 .Select(i => new ProductDTO { Id = i, Name = $"Product {i}",Barcode = $"Barcode {i}",Description = $"Description {i}", ClassRoomCode = $"ClassRoom {i}", QuantityInStock = i, QuantityOnOrder = i, LowStock = i, Categories = null, IsReservable = true ,IsHidden = false });
        products.Append(new ProductDTO { Id = 6, Name = "Product 6", Barcode = "Barcode 6", Description = "Description 6", ClassRoomCode = "ClassRoom 6", QuantityInStock = 10, QuantityOnOrder = 2, LowStock = 15, Categories = null, IsReservable = true ,IsHidden = false  });
        products.Append(new ProductDTO { Id = 6, Name = "Product 7", Barcode = "Barcode 7", Description = "Description 7", ClassRoomCode = "ClassRoom 7", QuantityInStock = 10, QuantityOnOrder = 2, LowStock = 10, Categories = null, IsReservable = true ,IsHidden = false  });

        return Task.FromResult(new ProductResponse { Products = products, TotalPages = 1 });
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
        return Task.FromResult(products.Products.Where(p => p.Barcode == barcode).First());
    }

    public Task<IEnumerable<ProductDTO>> GetProductsHavingLowStock()
    {
        var products = GetAllProducts(new ProductRequest.Index()).Result;
        return Task.FromResult(products.Products.Where(p => p.QuantityInStock + p.QuantityOnOrder < p.LowStock));
    }

    public Task AddProduct(ProductCreationDTO product)
    {
        throw new System.NotImplementedException();
    }

    public Task ToggleHideProduct(string barcode)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteProduct(string barcode)
    {
        throw new System.NotImplementedException();
    }
}

