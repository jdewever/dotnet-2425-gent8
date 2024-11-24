using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using BarcodeStandard;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Blazored.Toast.Services;

namespace Rise.Client.Products.AddProduct
{
    public partial class AddProduct
    {
        private BarcodeResponse barcode = null!;
        [Inject] public required ICategoryService CategoryService { get; set; }
        [Inject] public required IProductService ProductService { get; set; }
        [Inject] private IToastService ToastService { get; set; } = null!;

        private int selectedCategory;
        private bool showCategoryError = false;
        private bool showAddCategoryError = false;
        private string newCategoryName = string.Empty;
        private List<CategoryDTO> CategoryOptions = new List<CategoryDTO>();

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

        protected override async Task OnInitializedAsync()
        {
            var categories = await CategoryService.GetAllCategories();
            CategoryOptions = categories.ToList();

            barcode = await ProductService.GetNewBarcode();
            newProduct.Barcode = barcode.Barcode;
        }

        private async Task HandleValidSubmit()
        {
            await ProductService.AddProduct(newProduct);
            ToastService.ShowSuccess("Product succesvol toegevoegd!");

            newProduct = new ProductCreationDTO
            {
                Name = string.Empty,
                Description = string.Empty,
                Barcode = await ProductService.GetNewBarcode().ContinueWith(t => t.Result.Barcode),
                QuantityInStock = 0,
                QuantityOnOrder = 0,
                LowStock = 0,
                ClassRoomCode = string.Empty,
                IsReservable = false,
                IsHidden = false,
                CategoryIds = new List<int>()
            };

            selectedCategory = 0;
            newCategoryName = string.Empty;
            showCategoryError = false;
            showAddCategoryError = false;

            barcode = await ProductService.GetNewBarcode();
            newProduct.Barcode = barcode.Barcode;

            StateHasChanged(); 
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowError("Vergeet niet alle velden in te vullen!");
        }

        private void AddCategory()
        {
            if (newProduct.CategoryIds.Count == 3)
            {
                showCategoryError = true;
            }
            if (newProduct.CategoryIds.Count < 3 && selectedCategory > 0)
            {
                showCategoryError = false;
                newProduct.CategoryIds.Add(selectedCategory);
                selectedCategory = 0;
            }
        }

        private void RemoveCategory(int categoryId)
        {
            newProduct.CategoryIds.Remove(categoryId);
        }

        private async Task AddNewCategory()
        {
            if (string.IsNullOrWhiteSpace(newCategoryName))
            {
                showAddCategoryError = true;
            }
            else
            {
                showAddCategoryError = false;
                CategoryDTO newCategory = new CategoryDTO { Name = newCategoryName, Products = new List<ProductDTO>() };
                await CategoryService.AddCategory(newCategory);

                var updatedCategories = await CategoryService.GetAllCategories();
                CategoryOptions = updatedCategories.ToList();
                selectedCategory = CategoryOptions.FirstOrDefault(c => c.Name == newCategoryName)?.Id ?? 0;
                newCategoryName = string.Empty;

                StateHasChanged();
                ToastService.ShowSuccess("Categorie succesvol toegevoegd!");
            }

        }

        private async Task HandleFileSelected()
        {
            //todo -> adding image to product, blob?
        }
    }
}
