
using Microsoft.AspNetCore.Components;

namespace Rise.Client.Reservation;
public partial class BookingModal : ComponentBase
{
    [Parameter] public EventCallback HideModal { get; set; }

    [Parameter] public required List<int> BookedHours { get; set; }
    private List<HourRange> GetHourRanges(List<int> hours)
    {
        var ranges = new List<HourRange>();
        if (hours == null)
        {
            return ranges;
        }

        hours.Sort();
        int start = hours[0];
        int end = hours[0];

        for (int i = 1; i < hours.Count; i++)
        {
            if (hours[i] == end + 1)
            {
                end = hours[i];
            }
            else
            {
                ranges.Add(new HourRange { Start = start, End = end });
                start = hours[i];
                end = hours[i];
            }
        }
        ranges.Add(new HourRange { Start = start, End = end });
        return ranges;
    }

    private class HourRange
    {
        public int Start { get; set; }
        public int End { get; set; }
    }
}
