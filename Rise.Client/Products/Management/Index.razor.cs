
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Barcodes;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "barcode")]
    public string Barcode { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    // AddProduct params
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public IBarcodeService BarcodeService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    private int selectedCategoryID = -1;
    public required ProductDTO SelectedProduct { get; set; }
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private ProductCreationDTO newProduct = new();

    protected override async Task OnInitializedAsync()
    {
        SelectedProduct = await ProductService.GetProductByBarcode(Barcode ?? string.Empty);
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    private async Task HandleValidSubmit()
    {
        // TODO
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
        var categoryToAdd = CategoryOptions.FirstOrDefault(c => c.Id == selectedCategoryID);
        if (categoryToAdd != null && SelectedProduct.Categories != null && !SelectedProduct.Categories.Any(c => c == categoryToAdd))
        {
            SelectedProduct.Categories.Add(categoryToAdd);
        }
    }

    private void RemoveCategory(CategoryDTO category)
    {
        SelectedProduct.Categories!.Remove(category);
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