using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Buttons;
public partial class Button : ComponentBase
{
    [Parameter] public required string Text { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string ExtraCss { get; set; } = "";

    protected async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}