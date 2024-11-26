using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table;

// Action is a system reserved word, so we use TAction (TableAction) instead
public partial class TAction<T> : ComponentBase
{
    [Parameter] public required string Name { get; set; }
    [Parameter] public required EventCallback<T> Method { get; set; }
    [CascadingParameter] public required T Item { get; set; }

    private async Task OnClick()
    {
        await Method.InvokeAsync(Item);
    }
} 