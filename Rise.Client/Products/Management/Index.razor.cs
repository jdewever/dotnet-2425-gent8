
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Barcodes;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    public required ProductDTO SelectedProduct { get; set; }
    [Parameter]
    [SupplyParameterFromQuery(Name = "barcode")]
    public string? Barcode { get; set; }
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    // AddProduct params
    [Inject] public required ICategoryService CategoryService { get; set; }
    [Inject] public required IBarcodeService BarcodeService { get; set; }
    [Inject] private IToastService ToastService { get; set; } = null!;
    private int selectedCategory;
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private ProductCreationDTO newProduct = new();

    protected override async Task OnInitializedAsync()
    {
        SelectedProduct = await ProductService.GetProductByBarcode(Barcode ?? string.Empty);
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    // AddProduct methods
    private async Task HandleValidSubmit()
    {
        await ProductService.AddProduct(newProduct);
        newProduct = new ProductCreationDTO();
        StateHasChanged();
        ToastService.ShowSuccess("Product succesvol toegevoegd!");
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private void AddCategory()
    {
        var categoryToAdd = CategoryOptions.FirstOrDefault(c => c.Id == selectedCategory);
        if (categoryToAdd != null && SelectedProduct.Categories != null && !SelectedProduct.Categories.Any(c => c.Id == categoryToAdd.Id))
        {
            SelectedProduct.Categories.Add(categoryToAdd);
        }
    }

    private void RemoveCategory(int categoryId)
    {
        var categoryToRemove = CategoryOptions.FirstOrDefault(c => c.Id == categoryId);
        if (categoryToRemove != null && SelectedProduct.Categories != null)
        {
            SelectedProduct.Categories.Remove(categoryToRemove);
        }
    }

    private static void HandleFileSelected()
    {
        //todo -> adding image to product, blob?
    }

    private async Task HandleDelete()
    {
        await ProductService.DeleteProduct(SelectedProduct.Barcode);
        NavigationManager.NavigateTo("/products");
    }
}