using Microsoft.AspNetCore.Components;

namespace Rise.Client.Pages;

public partial class Authentication : ComponentBase
{
    [Parameter] public string? Action { get; set; }
}