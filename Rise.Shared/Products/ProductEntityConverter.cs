using Rise.Domain.DomainClasses;

namespace Rise.Shared.Products;

public static class ProductEntityConverter
{
    public static ProductDTO EntityToDto(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Barcode = product.Barcode,
            Description = product.Description,
            Name = product.Name,
            IsReservable = product.IsReservable,
            LowStock = product.LowStock,
            ClassRoomCode = product.ClassRoomCode,
            QuantityInStock = product.QuantityInStock,
            QuantityOnOrder = product.QuantityOnOrder,
            IsHidden = product.IsHidden,
            ImageUrl = product.ImageUrl,
            Categories = CategoryEntityConverter.CategoryEntityListToDtoList(product.Categories),
        };
    }
}