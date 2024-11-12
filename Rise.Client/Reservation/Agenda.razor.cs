using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;


namespace Rise.Client.Reservation;

public partial class Agenda : ComponentBase
{

    [Inject] NavigationManager? NavigationManager { get; set; }

    [Inject] IToastService? ToastService {get; set;}

    [Inject] IJSRuntime? JSRuntime {get; set;}


    public DateTime currentMonth = DateTime.Now;
    public bool isSelecting = false;
    public List<int> selectedDays = new List<int>();
    public int? firstSelectedDay = null;
    public int? lastSelectedDay = null;
    public bool isModalVisible = false;
    
    

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

    private void Reserve()
    {
        ToastService?.ShowSuccess("Reservatie succesvol aangemaakt");
        NavigationManager?.NavigateTo("/products");
        HideModal();
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
        var isPastDate = currentMonth.Year == DateTime.Now.Year && currentMonth.Month == DateTime.Now.Month && day < DateTime.Now.Day;
        if (isSelecting && !isPastDate)
        {
            if (!firstSelectedDay.HasValue)
            {
                firstSelectedDay = day;
            }
            lastSelectedDay = day;

            selectedDays.Clear();
            for (int i = Math.Min(firstSelectedDay.Value, lastSelectedDay.Value); i <= Math.Max(firstSelectedDay.Value, lastSelectedDay.Value); i++)
            {
                selectedDays.Add(i);
            }

            StateHasChanged(); 
        }
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