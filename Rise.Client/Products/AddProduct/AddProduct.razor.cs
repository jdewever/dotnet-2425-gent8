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

        private List<string> SelectedCategories = new List<string> { "" };
        private List<string> CategoryOptions = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var categories = await CategoryService.GetAllCategories();
            CategoryOptions = categories.Select(c => c.Name).ToList();
            CategoryOptions.Add("Andere");

            barcode = await ProductService.GetNewBarcode();
            image = await ProductService.GetBarcodeImage(barcode.Barcode);
        }

        private void AddCategory()
        {
            if (SelectedCategories.Count < 3)
            {
                SelectedCategories.Add("");
            }
        }

        private void RemoveCategory(int index)
        {
            if (index >= 0 && index <= SelectedCategories.Count)
            {
                SelectedCategories.RemoveAt(index-1);
            }
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

            await ProductService.AddProduct(newProduct);
        }

        private async Task HandleFileSelected()
        {

        }
    }
}
