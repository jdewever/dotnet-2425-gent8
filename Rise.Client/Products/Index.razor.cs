using Microsoft.AspNetCore.Components;
using Rise.Client.Scan;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDTO>? products;
    private IEnumerable<CategoryDTO>? categories;
    private IList<int>? _selectedCategories;
    private string? _searchTerm;
    private IEnumerable<string>? _locations;
    private bool isColumnTable = true;
    private int CurrentPage { get; set; } = 1;
    private int TotalPages { get; set; } = 1;
    private int PageSize { get; set; } = 14;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] ScanService ScanService { get; set; } = null!;

    private void SetCurrentPageToOne()
    {
        CurrentPage = 1;
    }

    private async Task showRegularTable()
    {
        isColumnTable = false;
        PageSize = 10;
        CurrentPage = 1;
        await OnParametersSetAsync();
    }

    private async Task showColumnTable()
    {
        isColumnTable = true;
        PageSize = 14;
        CurrentPage = 1;
        await OnParametersSetAsync();
    }

    private async void Callback(CategoryDTO obj)
    {
        await OnParametersSetAsync();
    }

    private async Task HandlePageChanged(int newPage)
    {
        CurrentPage = newPage;
        await OnParametersSetAsync();
    }

    private async Task HandlePageSizeChanged(int newPageSize)
    {
        PageSize = newPageSize;
        CurrentPage = 1;
        await OnParametersSetAsync();
    }

    [Inject] public required IProductService ProductService { get; set; }
    [Inject] public required ICategoryService CategoryService { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "Category")] public int[] SelectedCategoryList { get; set; } = [];
    [Parameter, SupplyParameterFromQuery(Name = "Location")] public string? Location { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "MaxInStock")] public int? MaxInStock { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "MinInStock")] public int? MinInStock { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "MaxOnOrder")] public int? MaxOnOrder { get; set; }
    [Parameter, SupplyParameterFromQuery(Name = "MinOnOrder")] public int? MinOnOrder { get; set; }
    [Parameter] public bool OnlyReservable { get; set; } = false;


    private async Task OnSearchInput(ChangeEventArgs e)
    {
        _searchTerm = e.Value?.ToString();
        CurrentPage = 1;
        await OnParametersSetAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        _selectedCategories = SelectedCategoryList.ToList();

        ProductRequest.Index request = new()
        {
            CategoryIds = SelectedCategoryList,
            Location = Location,
            MaxInStock = MaxInStock,
            MinInStock = MinInStock,
            MaxOnOrder = MaxOnOrder,
            MinOnOrder = MinOnOrder,
            Searchterm = _searchTerm,
            PageNumber = CurrentPage,
            PageSize = PageSize,
            OnlyReservable = OnlyReservable,
        };

        categories = await CategoryService.GetAllCategories();
        _locations = await ProductService.GetAllLocations();

        var productResponse = await ProductService.GetAllProducts(request);
        products = productResponse.Products;
        TotalPages = productResponse.TotalPages;
    }

    // table actions
    private void OnUitlenenClick(ProductDTO product)
    {
        ScanService.Barcode = product.Barcode;
        NavigationManager.NavigateTo("/scan");
    }
    private void OnReserverenClick(ProductDTO product)
    {
        selectedProduct = null; // hides the detail modal if it is was open
        selectedReservableProduct = product;
    }

    // modal actions
    private ProductDTO? selectedProduct;
    private void ShowModal(ProductDTO product)
    {
        selectedProduct = product;
    }

    private void CloseModal()
    {
        selectedProduct = null;
    }

    // reservation modal actions
    private ProductDTO? selectedReservableProduct;
    private void HideReservableProductsModal()
    {
        selectedReservableProduct = null;
    }

    private void OnProductClick(ProductDTO product)
    {
        if (OnlyReservable)
        {
            OnReserverenClick(product);
        }
        else
        {
            OnUitlenenClick(product);
        }
    }
}
