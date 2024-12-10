using System.Net.Http.Headers;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Rise.Shared.Barcodes;
using Rise.Shared.Products;

namespace Rise.Client.Products.AddProduct;

public partial class AddProduct : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBarcodeService BarcodeService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    public required ProductCreationDTO NewProduct { get; set; } = new();
    public required ProductCreationDTO InitProduct { get; set; } = new();
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private IBrowserFile? image;
    private CategoryDTO? selectedCategory;

    protected override async Task OnInitializedAsync()
    {
        CategoryOptions = await CategoryService.GetAllCategories();
        NewProduct.Barcode = (await BarcodeService.GetNewBarcode()).Barcode;
        InitProduct.Name = "Product naam";
        InitProduct.ClassRoomCode = "Lokaalcode";
        InitProduct.Description = "Product beschrijving";
    }

    private async Task HandleValidSubmit()
    {
        if (image is not null)
        {
            var fileStream = image.OpenReadStream();
            string imageUrl = await ProductService.UploadImage(fileStream, image.ContentType);
            NewProduct.ImageUrl = imageUrl;
        }

        await ProductService.AddProduct(NewProduct);
        if (NewProduct.IsReservable)
        {
            NavigationManager.NavigateTo("/reserve");
        }
        else
        {
            NavigationManager.NavigateTo("/products");
        }
        ToastService.ShowSuccess("Product succesvol toegevoegd!");
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private void HandleFileSelected(InputFileChangeEventArgs e)
    {
        image = e.File;
    }

    private void AddCategory()
    {
        if (NewProduct.CategoryTwoId == -1)
        {
            NewProduct.CategoryTwoId = -2;
            return;
        }
        else if (NewProduct.CategoryThreeId == -1)
        {
            NewProduct.CategoryThreeId = -2;
            return;
        }
    }

    // Adding new category
    public void ShowAddModal()
    {
        selectedCategory = new CategoryDTO { Id = -1, Name = string.Empty };
    }

    public void HideEditModal()
    {
        selectedCategory = null;
    }

    private async Task OnSave()
    {
        if (selectedCategory is not null)
            if (selectedCategory.Id == -1)
            {
                // new category
                await CategoryService.AddCategory(selectedCategory);
                ToastService.ShowSuccess($"Categorie {selectedCategory.Name} toegevoegd!");
            }
            else
            {
                await CategoryService.UpdateCategory(selectedCategory);
                ToastService.ShowSuccess($"Categorie {selectedCategory.Name} bijgewerkt!");
            }
        CategoryOptions = await CategoryService.GetAllCategories();
        HideEditModal();
    }
}
