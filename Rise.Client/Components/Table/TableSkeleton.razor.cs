using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Table
{
    public partial class TableSkeleton
    {
        [Parameter] public int ColumnCount { get; set; }
        [Parameter] public int RowCount { get; set; }
    }
}