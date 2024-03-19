namespace RexStudios.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DateTimeExtension
    {
        public static bool IsTodayOrInFuture(this DateTime date, DateTime endDate)
        {
            DateTime now = DateTime.Now;
            return (date.Date == now.Date || date > now) && date < endDate;
        }

        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        public static bool IsGreaterThan(this DateTime date, DateTime other)
        {
            return date > other;
        }

        public static bool IsLessThan(this DateTime date, DateTime other)
        {
            return date < other;
        }

        public static DateTime StartOfNextMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1);
        }

        public static IEnumerable<DateTime> Next36Months(this DateTime date)
        {
            DateTime nextDate = date.AddMonths(1).AddDays(-date.Day + 1);
            for (int i = 0; i < 36; i++)
            {
                yield return nextDate;
                nextDate = nextDate.AddMonths(1);
            }
        }
        public static DateTime Get36MonthDate(this DateTime date)
        {
            return date.AddMonths(36);
        }
        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1);
        }
    }
}
