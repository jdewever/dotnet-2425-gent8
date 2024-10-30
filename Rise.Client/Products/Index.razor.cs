using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Products;

public partial class Index
{
    private IEnumerable<ProductDTO>? products;
    private IEnumerable<CategoryDTO>? categories;
    private IList<int>? _selectedCategories;
    private string? _searchTerm;
    private IEnumerable<string>? _locations;
    private int totalPages;
    private Boolean table = false;
    private Boolean table2 = true;

    private int CurrentPage { get; set; } = 1;
    private int TotalPages { get; set; } = 1;
    private int PageSize { get; set; } = 14;

    private async Task showTable()
    {
        table = true;
        table2 = false;
        PageSize = 10;
        CurrentPage = 1;
        await OnParametersSetAsync();
    }

    private async Task showTable2()
    {
        table = false;
        table2 = true;
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
            PageSize = PageSize
        };

        categories = await CategoryService.GetAllCategories();
        _locations = await ProductService.GetAllLocations();

        var productResponse = await ProductService.GetAllProducts(request);
        products = productResponse.Products;
        TotalPages = productResponse.TotalPages;
    }
}
