using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products
{
    public partial class Pagination
    {
        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public int TotalPages { get; set; }
        [Parameter] public int CurrentPageSize { get; set; }
        [Parameter] public EventCallback<int> OnPageChanged { get; set; }
        [Parameter] public EventCallback<int> OnPageSizeChanged { get; set; }
        [Parameter] public bool table2 { get; set; }

        private bool IsFirstPage => CurrentPage <= 1;
        private bool IsLastPage => CurrentPage >= TotalPages;
        public int[] PageSizes => new int[] { 5, 10, 14, 20, 50, 100, 200 };

        private async Task GoToPreviousPage()
        {
            if (!IsFirstPage)
            {
                CurrentPage--;
                await OnPageChanged.InvokeAsync(CurrentPage);
            }
        }

        private async Task GoToNextPage()
        {
            if (!IsLastPage)
            {
                CurrentPage++;
                await OnPageChanged.InvokeAsync(CurrentPage);
            }
        }

        private async Task OnPageSizeChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newPageSize))
            {
                await OnPageSizeChanged.InvokeAsync(newPageSize);
            }
        }
    }
}