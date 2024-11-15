using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using BarcodeStandard;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rise.Client.Products.AddProduct
{
    public partial class AddProduct
    {
        private ProductCreationDTO newProduct = new ProductCreationDTO
        {
            Name = string.Empty,
            Description = string.Empty,
            Barcode = string.Empty,
            QuantityInStock = 0,
            QuantityOnOrder = 0,
            LowStock = 0,
            ClassRoomCode = string.Empty,
            IsReservable = false,
            IsHidden = false,
            CategoryIds = new List<int>()
        };

        private BarcodeResponse barcode = null!; 
        private string image = string.Empty; 

        [Inject] public required ICategoryService CategoryService { get; set; }
        [Inject] public required IProductService ProductService { get; set; }

        private int selectedCategory;
        private bool isAddButtonClicked = false;
        private List<CategoryDTO> CategoryOptions = new List<CategoryDTO>();

        protected override async Task OnInitializedAsync()
        {
            var categories = await CategoryService.GetAllCategories();
            CategoryOptions = categories.ToList();

            barcode = await ProductService.GetNewBarcode();
            image = await ProductService.GetBarcodeImage(barcode.Barcode);
        }

        private async Task HandleValidSubmit()
        {
            Console.WriteLine(newProduct.Name);
            Console.WriteLine(newProduct.ClassRoomCode);
            Console.WriteLine(newProduct.Description);
            Console.WriteLine(newProduct.Barcode);
            Console.WriteLine(newProduct.QuantityInStock);
            Console.WriteLine(newProduct.QuantityOnOrder);
            Console.WriteLine(newProduct.LowStock);
            Console.WriteLine(newProduct.IsReservable);
            Console.WriteLine(newProduct.IsHidden);

            Console.WriteLine("Category IDs:");
            foreach (var categoryId in newProduct.CategoryIds)
            {
                Console.WriteLine(categoryId);
            }

            await ProductService.AddProduct(newProduct);
        }

        private void AddCategory()
        {
            if (newProduct.CategoryIds.Count < 3 && selectedCategory > 0)
            {
                newProduct.CategoryIds.Add(selectedCategory);
                selectedCategory = 0; 
            }
        }

        private void RemoveCategory(int categoryId)
        {
            newProduct.CategoryIds.Remove(categoryId);
        }

        private async Task HandleFileSelected()
        {

        }
    }
}
