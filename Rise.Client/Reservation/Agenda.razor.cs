using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using Rise.Shared.Products;


namespace Rise.Client.Reservation;

public partial class Agenda : ComponentBase
{

    [Inject] NavigationManager? NavigationManager { get; set; }

    [Inject] IToastService? ToastService {get; set;}

    [Inject] IJSRuntime? JSRuntime {get; set;}

    [Inject] IProductService ProductService { get; set; } = null!;
    [Inject] IBookingService BookingService { get; set; } = null!;
    [Inject] BarcodeService BarcodeService { get; set; } = null!;


    public DateTime currentMonth = DateTime.Now;
    public bool isSelecting = false;
    public List<int> selectedDays = new List<int>();
    public int? firstSelectedDay = null;
    public int? lastSelectedDay = null;
    public bool isModalVisible = false;

    private ProductDTO? product;
    private List<BookingDTO>? bookings;

    protected override async Task OnInitializedAsync()
    {
        product = await ProductService.GetProductByBarcode(BarcodeService.Barcode);
        if (product != null)
        {
            bookings = await BookingService.GetBookingsByProductIdAsync(product.Id);
        }
    }
    
    

    public bool IsPreviousMonthDisabled => currentMonth.Year < DateTime.Now.Year || (currentMonth.Year == DateTime.Now.Year && currentMonth.Month <= DateTime.Now.Month);

    private void PreviousMonth()
    {
        if (!IsPreviousMonthDisabled)
        {
            currentMonth = currentMonth.AddMonths(-1);
            ClearSelection();
        }
    }

    private void NextMonth()
    {
        currentMonth = currentMonth.AddMonths(1);
        ClearSelection();
    }

    private void StartSelection()
    {
        isSelecting = true;
        ClearSelection();
    }

    private void EndSelection()
    {
        isSelecting = false;
    }

    private void ClearSelection()
    {
        selectedDays.Clear();
        firstSelectedDay = null;
        lastSelectedDay = null;
    }
    private void ShowModal()
    {
        isModalVisible = true;
    }

    private void HideModal()
    {
        isModalVisible = false;
    }
    private async Task Reserve()
    {
        if (firstSelectedDay.HasValue && lastSelectedDay.HasValue && product != null)
        {
            var startDate = new DateTime(currentMonth.Year, currentMonth.Month, firstSelectedDay.Value);
            var endDate = new DateTime(currentMonth.Year, currentMonth.Month, lastSelectedDay.Value);

            var booking = new BookingDTO
            {
                ProductId = product.Id,
                StartDate = startDate,
                EndDate = endDate,
                UserId = "TestId"
            };
            BarcodeService.Barcode = string.Empty;
            await BookingService.AddBookingAsync(booking);
            ToastService?.ShowSuccess("Reservatie succesvol aangemaakt");
            NavigationManager?.NavigateTo("/products");
            HideModal();
            await OnInitializedAsync(); 
        }
    }
    private string GetSelectedDateRange()
    {
        if (firstSelectedDay.HasValue && lastSelectedDay.HasValue)
        {
            var startDate = new DateTime(currentMonth.Year, currentMonth.Month, firstSelectedDay.Value);
            var endDate = new DateTime(currentMonth.Year, currentMonth.Month, lastSelectedDay.Value);
            return $"{startDate:MMMM d} - {endDate:MMMM d}";
        }
        return string.Empty;
    }
    private static void UpdateSelection(MouseEventArgs e)
    {
        // JavaScript Script
    }

    private void SelectDay(int day)
    {
        if (!IsValidDay(day))
        {
            return;
        }

        if (isSelecting && !IsPastDate(day))
        {
            if (!firstSelectedDay.HasValue)
            {
                firstSelectedDay = FindNextAvailableDay(day);
                if (!firstSelectedDay.HasValue)
                {
                    return;
                }
            }

            lastSelectedDay = day;

            selectedDays.Clear();
            bool encounteredBookedDate = false;
            for (int i = Math.Min(firstSelectedDay.Value, lastSelectedDay.Value); i <= Math.Max(firstSelectedDay.Value, lastSelectedDay.Value); i++)
            {
                if (IsBookedDay(i))
                {
                    encounteredBookedDate = true;
                    break;
                }
                selectedDays.Add(i);
            }

            if (encounteredBookedDate)
            {
                ResetSelectionToFirstDay();
            }

            StateHasChanged();
        }
    }

    private bool IsValidDay(int day)
    {
        return day >= 1 && day <= DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
    }

    private bool IsPastDate(int day)
    {
        return currentMonth.Year == DateTime.Now.Year && currentMonth.Month == DateTime.Now.Month && day < DateTime.Now.Day;
    }

    private bool IsBookedDay(int day)
    {
        var date = new DateTime(currentMonth.Year, currentMonth.Month, day);
        return bookings != null && bookings.Any(b => b.StartDate.Date <= date.Date && b.EndDate.Date >= date.Date);
    }

    private int? FindNextAvailableDay(int day)
    {
        while (IsBookedDay(day) && day <= DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month))
        {
            day++;
        }

        return day <= DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month) ? day : (int?)null;
    }

    private void ResetSelectionToFirstDay()
    {
        lastSelectedDay = firstSelectedDay;
        selectedDays.Clear();
        selectedDays.Add(firstSelectedDay!.Value);
    }


    [JSInvokable]
    public void UpdateSelection(int day)
    {
        SelectDay(day);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime!.InvokeVoidAsync("dragSelection.startSelection", DotNetObjectReference.Create(this));
        }
    }

    
}