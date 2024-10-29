using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Scan
{
    public partial class ScanTable
    {
        [Parameter] public required List<ProductDTO> Products { get; set; }
        [Inject]
        private HttpClient Http { get; set; }

        private async Task CheckoutCart()
        {
            var response = await Http.PostAsJsonAsync("Cart", new { /* Add necessary payload here */ });

            if (response.IsSuccessStatusCode)
            {
                // Handle success (e.g., navigate to a confirmation page, show a success message, etc.)
            }
            else
            {
                // Handle error (e.g., show an error message)
            }
        }
    }
}