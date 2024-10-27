using Microsoft.AspNetCore.Components;

namespace Rise.Client.Products
{
    public partial class Pagination
    {
        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public int TotalPages { get; set; }
        [Parameter] public EventCallback<int> OnPageChanged { get; set; }

        private bool IsFirstPage => CurrentPage <= 1;
        private bool IsLastPage => CurrentPage >= TotalPages;

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
    }
}