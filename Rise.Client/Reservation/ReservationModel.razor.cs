using Microsoft.AspNetCore.Components;

namespace Rise.Client.Reservation;
public partial class ReservationModal : ComponentBase {

  [Parameter] public required Func<string> GetSelectedDateRange { get; set; } = default!;
  [Parameter] public required EventCallback HideModal { get; set; }
  [Parameter] public required EventCallback Reserve { get; set; }

}


