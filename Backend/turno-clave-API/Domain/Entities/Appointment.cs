using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.DTOs.AppointmentItem;
using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public string ReservationCode { get; set; } = null!; // Unique code for client reference, can be generated as needed

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public int ClientId { get; set; }
        public required Client Client { get; set; }

        // Global start and end times for the appointment, stored in UTC
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<AppointmentItem> Items { get; set; } = [];

        public static AppointmentDTO ToDto(Appointment appt)
        {
            return new AppointmentDTO
            {
                ExternalId = appt.ExternalId,
                ReservationCode = appt.ReservationCode,
                BusinessExternalId = appt.Business.ExternalId,
                Client = ClientDTO.FromClient(appt.Client),
                StartDateTime = appt.StartDateTime,
                EndDateTime = appt.EndDateTime,
                Notes = appt.Notes,
                Status = appt.Status,
                CreatedAt = appt.CreatedAt,
                UpdatedAt = appt.UpdatedAt,
                Items = appt.Items.Select(item => new AppointmentItemDTO
                {
                    Service = MinimalServiceDTO.FromService(item.Service),
                    Professional = MinimalProfessionalDTO.FromProfessional(item.Professional),
                    StartDateTime = item.StartDateTime,
                    EndDateTime = item.EndDateTime,
                }).ToList()
            };
        }

        private static readonly char[] Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

        public static string GenerateReservationCode(int length = 6)
        {
            Random random = new();

            return new string(
                Enumerable.Range(0, length)
                    .Select(_ => Chars[random.Next(Chars.Length)])
                    .ToArray()
            );
        }
    }
}
