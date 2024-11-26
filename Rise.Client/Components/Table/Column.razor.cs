using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table;

public partial class Column<T> : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required string Value { get; set; }
    [CascadingParameter] public T? Item { get; set; }
} 