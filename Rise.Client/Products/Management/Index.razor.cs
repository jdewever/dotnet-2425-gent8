using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Rise.Shared.Products;

namespace Rise.Client.Products.Management;

public partial class Index
{
    [Parameter, SupplyParameterFromQuery(Name = "barcode")] public string Barcode { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    public required ProductCreationDTO SelectedProduct { get; set; }
    public required ProductDTO InitProduct { get; set; }
    private IEnumerable<CategoryDTO> CategoryOptions = [];
    private IBrowserFile? image;
    private CategoryDTO? selectedCategory;

    protected override async Task OnInitializedAsync()
    {
        InitProduct = await ProductService.GetProductByBarcode(Barcode ?? string.Empty);
        SelectedProduct = new(InitProduct);
        CategoryOptions = await CategoryService.GetAllCategories();
    }

    private async Task HandleValidSubmit()
    {
        // remove /api/proxy/image/ from the image url
        SelectedProduct.ImageUrl = SelectedProduct.ImageUrl.Replace("/api/proxy/image/", "");
        // decode the image url
        SelectedProduct.ImageUrl = System.Net.WebUtility.UrlDecode(SelectedProduct.ImageUrl);

        if (image is not null)
        {
            var fileStream = image.OpenReadStream(20 * 1024 * 1024); // 20MB
            string imageUrl = await ProductService.UploadImage(fileStream, image.ContentType);
            SelectedProduct.ImageUrl = imageUrl;
        }

        await ProductService.UpdateProduct(SelectedProduct.Barcode, SelectedProduct);
        StateHasChanged();
        ToastService.ShowSuccess("Product succesvol gewijzigd!");
        if (SelectedProduct.IsHidden)
        {
            NavigationManager.NavigateTo("/manage/hiddenproducts");
        }
        else if (SelectedProduct.IsReservable)
        {
            NavigationManager.NavigateTo("/reserve");
        }
        else
        {
            NavigationManager.NavigateTo("/products");
        }

    }

    private void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }

    private void HandleFileSelected(InputFileChangeEventArgs e)
    {
        image = e.File;
    }

    private async Task HandleDelete()
    {
        await ProductService.DeleteProduct(SelectedProduct.Barcode);
        if (SelectedProduct.IsReservable)
        {
            NavigationManager.NavigateTo("/reserve");
        }
        else
        {
            NavigationManager.NavigateTo("/products");
        }
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