using Microsoft.AspNetCore.Components;

namespace Rise.Client.Manage.Components;

public partial class DeleteModal : ComponentBase
{
    [Parameter] public required string ItemType { get; set; }
    [Parameter] public required string ItemName { get; set; }
    [Parameter] public required EventCallback OnConfirm { get; set; }
    [Parameter] public required EventCallback OnClose { get; set; }

    private async Task OnCloseClick()
    {
        await OnClose.InvokeAsync();
    }

    private async Task OnConfirmClick()
    {
        await OnConfirm.InvokeAsync();
    }
}