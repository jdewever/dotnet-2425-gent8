using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table
{
    public partial class ReservationsTableSkeleton
    {
        [Parameter] public int ColumnCount { get; set; }
        [Parameter] public int RowCount { get; set; }
    }
}