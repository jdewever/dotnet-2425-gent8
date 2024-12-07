
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Barcodes;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Parameter, SupplyParameterFromQuery(Name = "barcode")] public string Barcode { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public IBarcodeService BarcodeService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    public required ProductCreationDTO SelectedProduct { get; set; }
    public required ProductDTO InitProduct { get; set; }
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private CategoryDTO? selectedCategory;

    protected override async Task OnInitializedAsync()
    {
        InitProduct = await ProductService.GetProductByBarcode(Barcode ?? string.Empty);
        SelectedProduct = new(InitProduct);
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    private async Task HandleValidSubmit()
    {
        await ProductService.UpdateProduct(SelectedProduct.Barcode, SelectedProduct);
        StateHasChanged();
        ToastService.ShowSuccess("Product succesvol gewijzigd!");
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private static void HandleFileSelected()
    {
        //todo -> adding image to product, blob?
    }

    private async Task HandleDelete()
    {
        await ProductService.DeleteProduct(SelectedProduct.Barcode);
        NavigationManager.NavigateTo("/products");
        ToastService.ShowSuccess("Product succesvol verwijderd!");
    }

    private void AddCategory()
    {
        if (SelectedProduct.CategoryTwoId == -1)
        {
            SelectedProduct.CategoryTwoId = -2;
            return;
        }
        else if (SelectedProduct.CategoryThreeId == -1)
        {
            SelectedProduct.CategoryThreeId = -2;
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