using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using BarcodeStandard;
using SkiaSharp;

namespace Rise.Client.Products.AddProduct
{
    public partial class AddProduct
    {
        private ProductDTO newProduct = new ProductDTO
        {
            Id = 0,
            Name = string.Empty,
            Description = string.Empty,
            Barcode = string.Empty,
            QuantityInStock = 0,
            QuantityOnOrder = 0,
            LowStock = 0,
            ClassRoomCode = string.Empty,
            IsReservable = false,
            Categories = new List<CategoryDTO>()
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

        }

        private async Task HandleFileSelected()
        {
            
        }
    }
}
