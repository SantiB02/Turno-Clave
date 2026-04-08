using System;
using System.Linq;
using turno_clave_API.Common;

namespace turno_clave_API.Infrastructure.Time
{
    public static class TimeZoneHelper
    {
        public static Result<string> NormalizeTimeZoneId(string timeZone)
        {
            if (string.IsNullOrWhiteSpace(timeZone))
                return Result<string>.Success(TimeZoneInfo.Utc.Id);

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return Result<string>.Success(tz.Id);
            }
            catch (TimeZoneNotFoundException)
            {
                var match = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(t => string.Equals(t.Id, timeZone, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(t.StandardName, timeZone, StringComparison.OrdinalIgnoreCase)
                        || (!string.IsNullOrEmpty(t.DisplayName) && t.DisplayName.IndexOf(timeZone, StringComparison.OrdinalIgnoreCase) >= 0));

                if (match != null)
                    return Result<string>.Success(match.Id);
                else return Result<string>.Failure($"Invalid time zone identifier: {timeZone}");
            }
        }

        //public static DateTimeOffset ConvertDateAndTimeToDateTimeOffset(DateOnly date, TimeOnly time, string timeZoneId)
        //{
        //    var tzIdResult = NormalizeTimeZoneId(timeZoneId);
        //    var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);

        //    var local = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Unspecified);
        //    var offset = tz.GetUtcOffset(local);
        //    return new DateTimeOffset(local, offset);
        //}
    }
}
