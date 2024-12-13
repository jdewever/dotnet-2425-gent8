using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components;

public partial class Card : ComponentBase
{
    [Parameter] public required string IconUrl { get; set; }
    [Parameter] public required string Title { get; set; }
    [Parameter] public required string Value { get; set; }
    [Parameter] public required string ID { get; set; }
}