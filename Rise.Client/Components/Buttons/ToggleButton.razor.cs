using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Buttons
{
    public partial class ToggleButton
    {
        [Parameter] public string Text { get; set; } = "Geschiedenis";
        [Parameter] public EventCallback OnClick { get; set; }

        private async Task HandleClick()
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync();
            }
        }
    }
}