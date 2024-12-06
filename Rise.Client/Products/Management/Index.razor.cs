
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
    private bool showCategoryError = false;
    private bool showAddCategoryError = false;
    private string newCategoryName = string.Empty;
    private BarcodeResponse barcode = null!;
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

    private string GetQueryParm(string parmName)
    {
        var uriBuilder = new UriBuilder(NavigationManager.Uri);
        var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        return q[parmName] ?? "";
    }

    protected override async Task OnInitializedAsync()
    {
        SelectedProduct = await ProductService.GetProductByBarcode(GetQueryParm("barcode"));
    }

    // AddProduct methods
    private async Task HandleValidSubmit()
    {
        await ProductService.AddProduct(newProduct);
        ToastService.ShowSuccess("Product succesvol toegevoegd!");

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

        selectedCategory = 0;
        newCategoryName = string.Empty;
        showCategoryError = false;
        showAddCategoryError = false;

        barcode = await BarcodeService.GetNewBarcode();
        newProduct.Barcode = barcode.Barcode;

        StateHasChanged();
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

    private static void HandleFileSelected()
    {
        //todo -> adding image to product, blob?
    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }
}