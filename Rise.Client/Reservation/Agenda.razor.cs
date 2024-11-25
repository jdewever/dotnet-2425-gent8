using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Rise.Shared.Products;
using Blazored.Toast.Services;
using Rise.Client.Scan;

namespace Rise.Client.Reservation
{
    public partial class Agenda
    {
        private DateTime currentDate = DateTime.Now;
        private int? startHour = null;
        private int? endHour = null;
        private bool isSelecting = false;
        private DateTime startDay = DateTime.Now;
        private DateTime endDay = DateTime.Now;
        private ProductDTO? product;
        private List<BookingDTO>? bookings;
        private Dictionary<DateTime, List<int>> bookedHoursByDate = new Dictionary<DateTime, List<int>>();

        private bool hasConflict = false;

        private bool noHourSelected = false;

        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] private ScanService ScanService { get; set; } = null!;
        [Inject] private IProductService ProductService { get; set; } = null!;
        [Inject] private IBookingService BookingService { get; set; } = null!;
        [Inject] private IToastService ToastService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (ScanService.Barcode == null)
            {
                NavigationManager.NavigateTo("/scan");
                return;
            }

            product = await ProductService.GetProductByBarcode(ScanService.Barcode);
            if (product != null)
            {
                bookings = await BookingService.GetBookingsByProductIdAsync(product.Id);
                InitializeBookedHours();
            }
        }
        private void InitializeBookedHours()
        {
            if (bookings == null) return;

            foreach (var booking in bookings)
            {
                DateTime current = booking.StartDate;
                while (current <= booking.EndDate)
                {
                    if (!bookedHoursByDate.ContainsKey(current.Date))
                    {
                        bookedHoursByDate[current.Date] = new List<int>();
                    }

                    int startHour = current.Date == booking.StartDate.Date ? booking.StartDate.Hour : 0;
                    int endHour = current.Date == booking.EndDate.Date ? booking.EndDate.Hour : 23;

                    if (current.Date == booking.EndDate.Date && booking.StartDate.Hour > booking.EndDate.Hour)
                    {
                        endHour = 23;
                    }

                    for (int hour = startHour; hour <= endHour; hour++)
                    {
                        if (!bookedHoursByDate[current.Date].Contains(hour))
                        {
                            bookedHoursByDate[current.Date].Add(hour);
                        }
                    }

                    current = current.AddDays(1);

                    if (current.Date == booking.EndDate.Date && booking.StartDate.Hour > booking.EndDate.Hour)
                    {
                        if (!bookedHoursByDate.ContainsKey(current.Date))
                        {
                            bookedHoursByDate[current.Date] = new List<int>();
                        }

                        for (int hour = 0; hour <= booking.EndDate.Hour; hour++)
                        {
                            if (!bookedHoursByDate[current.Date].Contains(hour))
                            {
                                bookedHoursByDate[current.Date].Add(hour);
                            }
                        }
                    }
                }
            }
        }

        private async Task Reserve()
        {
        
            if (product != null)
            {
                if(startHour == null || endHour == null)
                {
                    noHourSelected = true;
                    return;
                }else
                {
                    noHourSelected = false;
                }
                DateTime startDate = new DateTime(startDay.Year, startDay.Month, startDay.Day, startHour ?? 0, 0, 0);
                DateTime endDate = new DateTime(endDay.Year, endDay.Month, endDay.Day, endHour ?? 0, 0, 0);
                
                DateTime current = startDay;
                
                while (current <= endDay)
                {
                    int startHourToCheck = current == startDay ? startHour ?? 0 : 0;
                    int endHourToCheck = current == endDay ? endHour ?? 23 : 23;

                    if (bookedHoursByDate.TryGetValue(current.Date, out var bookedHours))
                    {
                        for (int hour = startHourToCheck; hour <= endHourToCheck; hour++)
                        {
                            if (bookedHours.Contains(hour))
                            {
                                hasConflict = true;
                                break;
                            }
                        }
                    }

                    if (hasConflict)
                    {
                        break;
                    }

                    current = current.AddDays(1);
                }

                if (hasConflict)
                {
                    return;
                }
                var booking = new BookingDTO
                {
                    Product = product,
                    StartDate = startDate,
                    EndDate = endDate,
                    UserId = "Test1"
                };
                
                await BookingService.AddBookingAsync(booking);
                ToastService?.ShowSuccess("Reservatie succesvol aangemaakt");
                NavigationManager?.NavigateTo("/reserve");
                await OnInitializedAsync();
                
                
                
            }
        }
        private bool HasBookings(DateTime date)
        {
            return bookings?.Any(b => b.StartDate.Date <= date.Date && b.EndDate.Date >= date.Date) ?? false;
        }
        
        private int getDaysInMonth()
        {
            return DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
        }
        private void PreviousMonth()
        {
            DateTime previousMonthDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);

            if (previousMonthDate.Month == DateTime.Now.Month && previousMonthDate.Year == DateTime.Now.Year)
            {
                currentDate = DateTime.Now;
            }
            else
            {
                currentDate = previousMonthDate;
            }

            startHour = null;
            endHour = null;
            startDay = currentDate;
            endDay = currentDate;
        }

        private void NextMonth()
        {
            currentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
            startHour = null;
            endHour = null;
            startDay = currentDate;
            endDay = currentDate;
        }
        private void PreviousDate()
        {
            currentDate = currentDate.AddDays(-1);
            endHour = null;
            startHour = null;
            startDay = currentDate;
            endDay = currentDate;
        }

        private void NextDate()
        {
            currentDate = currentDate.AddDays(1);
            startDay = currentDate;
            endDay = currentDate;
            endHour = null;
            startHour = null;
        }

        private string GetHourClass(int hour)
        {
            return startHour == null ? "" :
                endHour == null ? (hour == startHour ? "bg-blue-300 rounded-t-lg rounded-b-lg" : "") :
                hour == startHour && hour == endHour ? "bg-blue-300 rounded-t-lg rounded-b-lg" :
                hour == startHour ? "bg-blue-300 rounded-t-lg" :
                hour == endHour ? "bg-blue-300 rounded-b-lg" :
                (hour > startHour && hour < endHour) ? "bg-blue-300" : "";
        }
        private string GetDayClass(DateTime day)
        {
            if (DateTime.Compare(startDay.Date, day.Date) == 0 && DateTime.Compare(endDay.Date, day.Date) == 0)
            {
                return "bg-blue-300 rounded-t-lg rounded-b-lg";
            }

            if (DateTime.Compare(startDay.Date, day) == 0)
            {
                return "bg-blue-300 rounded-l-lg";
            }

            if (DateTime.Compare(endDay.Date, day.Date) == 0)
            {
                return "bg-blue-300 rounded-r-lg";
            }

            return DateTime.Compare(startDay.Date, day.Date) < 0 && DateTime.Compare(endDay.Date, day.Date) > 0 ? "bg-blue-300" : "";
        }
        private string GetHourSelectableClass(int hour)
        {
            if (bookedHoursByDate.TryGetValue(currentDate.Date, out var bookedHours) && DateTime.Compare(startDay.Date, endDay.Date) == 0)
            {
                return bookedHours.Contains(hour) ? "bg-gray-100 rounded cursor-not-allowed" : "cursor-pointer";
            }
            return DateTime.Compare(startDay.Date,endDay.Date) != 0 ? "bg-gray-100 rounded cursor-not-allowed" : "cursor-pointer";
        }

        [JSInvokable]
        public void StartSelection()
        {
            if (startDay != endDay)
            {
                return;
            }

            isSelecting = true;
            startHour = null;
            endHour = null;
            StateHasChanged();
        }

        [JSInvokable]
        public void EndSelection()
        {
            isSelecting = false;
            StateHasChanged();
        }

        [JSInvokable]
        public void UpdateSelection(int hour)
        {
            if (isSelecting)
            {
                if (bookedHoursByDate.TryGetValue(currentDate.Date, out var bookedHours) && bookedHours.Contains(hour))
                {
                    EndSelection();
                    return;
                }
                if (startHour == null)
                {
                    startHour = hour;
                }
                else
                {
                    if(hour < startHour)
                    {
                        if(endHour == null)
                        {
                            endHour = startHour;
                            startHour = hour;
                        }else
                        {
                            startHour = hour;
                        }
                    }else if(hour > startHour)
                    {
                        endHour = hour;
                    }
                }
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("dragSelection.startSelection", DotNetObjectReference.Create(this));
                await JSRuntime.InvokeVoidAsync("dragSelectDays.initialize", DotNetObjectReference.Create(this));
            }
        }
        private void SelectHour(int hour)
        {
            if (startDay != endDay)
            {
                return;
            }
            if (bookedHoursByDate.TryGetValue(currentDate.Date, out var bookedHours) && bookedHours.Contains(hour))
            {
                return;
            }
            startHour = hour;
            endHour = hour;
            StateHasChanged();
        }
        private void SelectDay(DateTime date, bool isPastDay)
        {
            if (isPastDay)
            {
                return;   
            }

            currentDate = date;
            startDay = date;
            endDay = date;
            endHour = null;
            startHour = null;
            StateHasChanged();
        }
        
        [JSInvokable]
        public void StartDaySelection(DateTime day)
        {
            if (day < DateTime.Today)
            {
                EndDaySelection();
                return;
            }
            isSelecting = true;
            startDay = day;
            endDay = startDay;
            startHour = null;
            endHour = null;
            hasConflict = false;
            noHourSelected = false;
            StateHasChanged();
        }

        [JSInvokable]
        public void EndDaySelection()
        {
            isSelecting = false;
            StateHasChanged();
        }

        [JSInvokable]
        public void UpdateDaySelection(DateTime day)
        {
            if (isSelecting)
            {
                if (day < DateTime.Today)
                {
                    return;
                }
                if(day < startDay)
                {
                    startDay = day;
                }else if(day > startDay)
                {
                    endDay = day;
                }
                StateHasChanged();
            }
        }
    }
}
