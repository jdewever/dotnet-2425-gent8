using System;

namespace Rise.Client.Pages.Transaction
{
    public class Common
    {
        public static DateTime GetRoundedDate(int daysDifference, int hoursToAdd)
        {
            var date = DateTime.Now.AddDays(daysDifference).AddHours(hoursToAdd);
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }
    }
}