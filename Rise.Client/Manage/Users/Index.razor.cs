using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Rise.Client.Auth;
using Rise.Shared.User;

namespace Rise.Client.Manage.Users;
public partial class Index : ComponentBase
{
    [Parameter] public ManageView<UserDTO>? ManageViewComponent { get; set; }
    [Inject] private ILocalUserService LocalUserService { get; set; } = null!;
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private IEnumerable<UserDTO>? Users;
    private UserCreationDTO? selectedUser;

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetUsers();
    }
    private async Task HandleBlock(UserDTO user)
    {
        var updatedUser = await UserService.BlockUser(user.UserID);
        Users = Users?.Select(u => u.UserID == updatedUser.UserID ? updatedUser : u).ToList();
        StateHasChanged();
        ToastService.ShowSuccess($"Gebruiker {user.FullName} is {(user.IsBlocked ? "gedeblokkeerd" : "geblokkeerd")}.");
    }

    private async Task HandleDelete(UserDTO user)
    {
        // don't delete the current user
        var currentUser = await LocalUserService.GetCurrentUser();
        if (user.UserID == currentUser.UserID)
        {
            ToastService.ShowError("Je kunt jezelf niet verwijderen.");
            return;
        }

        var result = await UserService.DeleteUser(user.UserID);
        if (result)
        {
            Users = Users?.Where(u => u.UserID != user.UserID).ToList();
            StateHasChanged();
            ToastService.ShowSuccess($"Gebruiker {user.FullName} is verwijderd.");
        }
        else
        {
            ToastService.ShowError($"Gebruiker {user.FullName} kon niet worden verwijderd.");
        }
    }

    private void HandleAdd()
    {
        selectedUser = new UserCreationDTO { FirstName = "", LastName = "", Email = "", Password = "", Role = "" };
    }
    private static MarkupString GetUserState(UserDTO user)
    {
        return new($"<span class='nline-flex items-center rounded-md px-2 py-1 text-xs font-medium ring-1 ring-inset {(user.IsBlocked ? "text-red-700 ring-red-600/10 bg-red-50" : "text-green-700 ring-green-600/20 bg-green-50" )}'>{(user.IsBlocked ? "Geblokkeerd" : "Actief" )}</span>");
    }

    private async Task OnSave()
    {
        if (selectedUser is null)
            return;

        await UserService.AddUser(selectedUser);
        ToastService.ShowSuccess($"Gebruiker {selectedUser.FirstName} toegevoegd!");

        Users = await UserService.GetUsers();
        HideEditModal();
    }

    private void HideEditModal()
    {
        selectedUser = null;
    }
}