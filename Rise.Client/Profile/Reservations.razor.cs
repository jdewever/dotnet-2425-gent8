using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Profile;

public partial class Reservations : ComponentBase
{
    [Parameter, EditorRequired] public required IEnumerable<BookingDTO> BookingsList { get; set; }
}