using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rise.Client.Products.AddProduct
{
    public partial class AddProduct
    {
        [Inject] public required ICategoryService CategoryService { get; set; }
       // private ProductDTO ProductModel { get; set; } = new ProductDTO();
        private List<string> SelectedCategories = new List<string> { "" };
        private List<string> CategoryOptions = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var categories = await CategoryService.GetAllCategories();
            CategoryOptions = categories.Select(c => c.Name).ToList();
        }

        private void AddCategory()
        {
            SelectedCategories.Add("");
        }

        private void RemoveCategory(int index)
        {
            if (index >= 0 && index < SelectedCategories.Count)
            {
                SelectedCategories.RemoveAt(index);
            }
        }

        private async Task HandleValidSubmit()
        {

        }
    }
}
