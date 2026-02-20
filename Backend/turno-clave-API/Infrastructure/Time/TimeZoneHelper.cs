using System;
using System.Linq;

namespace turno_clave_API.Infrastructure.Time
{
    /// <summary>
    /// Helper utilities for timezone validation and conversions.
    /// </summary>
    public static class TimeZoneHelper
    {
        public static string NormalizeTimeZoneId(string timeZone)
        {
            if (string.IsNullOrWhiteSpace(timeZone))
                return TimeZoneInfo.Utc.Id;

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return tz.Id;
            }
            catch
            {
                var match = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(t => string.Equals(t.Id, timeZone, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(t.StandardName, timeZone, StringComparison.OrdinalIgnoreCase)
                        || (!string.IsNullOrEmpty(t.DisplayName) && t.DisplayName.IndexOf(timeZone, StringComparison.OrdinalIgnoreCase) >= 0));

                if (match != null)
                    return match.Id;

                throw new ArgumentException($"Invalid time zone identifier: {timeZone}");
            }
        }

        public static DateTimeOffset ConvertDateAndTimeToDateTimeOffset(DateOnly date, TimeOnly time, string timeZoneId)
        {
            var tzId = NormalizeTimeZoneId(timeZoneId);
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);

            var local = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Unspecified);
            var offset = tz.GetUtcOffset(local);
            return new DateTimeOffset(local, offset);
        }
    }
}
