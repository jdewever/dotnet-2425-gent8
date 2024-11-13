
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products.Management;

public partial class Index
{
    public string Barcode { get; set; } = default!;

    [Inject] private BarcodeService BarcodeService { get; set; } = null!;

    private string selectedButton = "Wijzigen";
    public void SelectButton(string page)
    {
        selectedButton = page;
    }

    private string GetButtonClass(string buttonName)
    {
        return $"px-3 text-gray-600 rounded-lg {(selectedButton == buttonName ? "bg-gray-300 rounded-lg" : "")}";
    }

    protected override Task OnInitializedAsync()
    {
        BarcodeService.Barcode = Barcode;
        return Task.CompletedTask;
    }
}