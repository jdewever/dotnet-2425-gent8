using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table;

public partial class Table<T> : ComponentBase
{
    [Parameter] public required IEnumerable<T> Items { get; set; }
    [Parameter] public RenderFragment? Columns { get; set; }
    [Parameter] public RenderFragment? Actions { get; set; } = null;

    [Parameter] public EventCallback<T>? OnSelectRow { get; set; }

    private void SelectItem(T item)
    {
        if (OnSelectRow != null)
        {
            OnSelectRow.Value.InvokeAsync(item);
        }
    }
}