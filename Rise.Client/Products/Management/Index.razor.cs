
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Barcodes;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    public ProductDTO SelectedProduct { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;

    // AddProduct params
    [Inject] public required ICategoryService CategoryService { get; set; }
    [Inject] public required IBarcodeService BarcodeService { get; set; }
    [Inject] private IToastService ToastService { get; set; } = null!;
    private int selectedCategory;
    private bool showAddCategoryError = false;
    private string newCategoryName = string.Empty;
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private ProductCreationDTO newProduct = new()
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
        CategoryIds = []
    };

    private string GetQueryParm(string parmName)
    {
        var uriBuilder = new UriBuilder(NavigationManager.Uri);
        var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return q[parmName] ?? "";
    }

    protected override async Task OnInitializedAsync()
    {
        SelectedProduct = await ProductService.GetProductByBarcode(GetQueryParm("barcode"));
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    // AddProduct methods
    private async Task HandleValidSubmit()
    {
        await ProductService.AddProduct(newProduct);

        newProduct = new ProductCreationDTO
        {
            Name = string.Empty,
            Description = string.Empty,
            Barcode = await BarcodeService.GetNewBarcode().ContinueWith(t => t.Result.Barcode),
            QuantityInStock = 0,
            QuantityOnOrder = 0,
            LowStock = 0,
            ClassRoomCode = string.Empty,
            IsReservable = false,
            IsHidden = false,
            CategoryIds = new List<int>()
        };

        StateHasChanged();
        ToastService.ShowSuccess("Product succesvol toegevoegd!");
    }

    private void AddCategory()
    {
        newProduct.CategoryIds.Add(selectedCategory);
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
            CategoryDTO newCategory = new() { Name = newCategoryName, Products = [] };
            await CategoryService.AddCategory(newCategory);

            CategoryOptions = await CategoryService.GetAllCategories();
            selectedCategory = CategoryOptions.FirstOrDefault(c => c.Name == newCategoryName)?.Id ?? 0;
            newCategoryName = string.Empty;

            StateHasChanged();
            ToastService.ShowSuccess("Categorie succesvol toegevoegd!");
        }

    }

    private static void HandleFileSelected()
    {
        //todo -> adding image to product, blob?
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private async Task HandleDelete()
    {
        await ProductService.DeleteProduct(SelectedProduct.Barcode);
        NavigationManager.NavigateTo("/products");
    }
}