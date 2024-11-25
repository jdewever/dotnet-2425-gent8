using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Buttons
{
        public partial class BigButton
        {
            [Parameter] public string Text { get; set; } = "Uitscannen";
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