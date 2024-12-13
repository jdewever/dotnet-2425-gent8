using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Shared.User;

namespace Rise.Client.Manage.Users;
public partial class Index : ComponentBase
{
    [Parameter] public ManageView<UserDTO>? ManageViewComponent { get; set; }
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private IEnumerable<UserDTO>? Users;
    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetUsers();
    }
    private async Task HandleBlock(UserDTO user)
    {
        await UserService.BlockUser(user.UserID);
        StateHasChanged();
        ToastService.ShowSuccess($"Gebruiker {user.FullName} is {(user.IsBlocked ? "gedeblokkeerd" : "geblokkeerd")}.");
    }

    private void NavigateToAddProduct()
    {
        NavigationManager.NavigateTo("/products/add");
    }

    // private void HandleEdit(ProductDTO product)
    // {
    //     NavigationManager.NavigateTo($"/products/management/?barcode={product.Barcode}");
    // }

    private static MarkupString GetUserState(UserDTO user)
    {
        return new($"<span class='nline-flex items-center rounded-md px-2 py-1 text-xs font-medium ring-1 ring-inset {(user.IsBlocked ? "text-red-700 ring-red-600/10 bg-red-50" : "text-green-700 ring-green-600/20 bg-green-50" )}'>{(user.IsBlocked ? "Geblokkeerd" : "Actief" )}</span>");
    }
}