using Microsoft.AspNetCore.Components;
using Rise.Shared.Products;

namespace Rise.Client.Pages.Reservation.Components;

public partial class ReservationsTable : ComponentBase
{
    [Parameter] public IEnumerable<BookingDTO>? Reservations { get; set; }
    [Parameter] public EventCallback<BookingDTO> OnCancel { get; set; }
    [Parameter] public bool ShowHistory { get; set; }
}