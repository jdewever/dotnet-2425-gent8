
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
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public IBarcodeService BarcodeService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    private int selectedCategoryID = -1;
    public required ProductCreationDTO SelectedProduct { get; set; }
    public required ProductDTO InitProduct { get; set; }
    private IEnumerable<CategoryDTO> CategoryOptions = [];

    protected override async Task OnInitializedAsync()
    {
        InitProduct = await ProductService.GetProductByBarcode(Barcode ?? string.Empty);
        SelectedProduct = new(InitProduct);
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    private async Task HandleValidSubmit()
    {
        // TODO
        await Task.Delay(2000);
        StateHasChanged();
        ToastService.ShowSuccess("Product succesvol toegevoegd!");
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private void AddCategory()
    {
        if (!SelectedProduct.CategoryIds.Contains(selectedCategoryID))
        {
            SelectedProduct.CategoryIds = [.. SelectedProduct.CategoryIds, selectedCategoryID];
        }
    }

    private void RemoveCategory(int category)
    {
        SelectedProduct.CategoryIds.Remove(category);
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