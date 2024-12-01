using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Manage.Components;

public partial class EditModal<T> : ComponentBase
{
    [Parameter] public required T Item { get; set; }
    [Parameter] public required EventCallback OnConfirm { get; set; }
    [Parameter] public required EventCallback OnClose { get; set; }
    [Inject] private IToastService ToastService { get; set; } = null!;

    protected T newItem = default!;

    protected async Task OnCloseClick()
    {
        await OnClose.InvokeAsync();
    }

    protected async Task HandleValidSubmit()
    {
        await OnConfirm.InvokeAsync();
        StateHasChanged();
    }

    protected void HandleInvalidSubmit()
    {
        ToastService.ShowError("Vergeet niet alle velden in te vullen!");
    }
}