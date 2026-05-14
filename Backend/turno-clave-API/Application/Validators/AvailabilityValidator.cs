using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Validators
{
    public static class AvailabilityValidator
    {
        public static bool HasOverlappingAvailabilities(IEnumerable<AvailabilityRange> availabilities)
        {
            var groupedByDay = availabilities
                .GroupBy(a => a.DayOfWeek);

            foreach (var dayGroup in groupedByDay)
            {
                List<AvailabilityRange> ordered = dayGroup
                    .OrderBy(a => a.StartTime)
                    .ToList();

                for (int i = 0; i < ordered.Count - 1; i++)
                {
                    AvailabilityRange current = ordered[i];
                    AvailabilityRange next = ordered[i + 1];

                    bool overlaps = current.EndTime > next.StartTime;

                    if (overlaps)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
