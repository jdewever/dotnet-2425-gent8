using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table;

// Action is a system reserved word even used in this class, so we use TAction (TableAction) instead
public partial class TAction<T> : ComponentBase
{
    [Parameter] public required string Name { get; set; }
    [Parameter] public required Action<T> Method { get; set; }
    [CascadingParameter] public required T Item { get; set; }
} 