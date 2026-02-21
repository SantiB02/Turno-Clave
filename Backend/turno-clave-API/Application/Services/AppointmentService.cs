using System;
using System.Linq;
using System.Threading.Tasks;
using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace turno_clave_API.Application.Services
{
    public class AppointmentService(AppDbContext context) : IAppointmentService
    {
        private readonly AppDbContext _context = context;

        public async Task<Appointment?> CreateAsync(CreateAppointmentDTO dto)
        {
            Business? business = _context.Businesses.FirstOrDefault(b => b.ExternalId.ToString() == dto.BusinessExternalId);
            if (business == null) return null;

            Professional? professional = _context.Professionals.FirstOrDefault(p => p.ExternalId.ToString() == dto.ProfessionalExternalId);
            if (professional == null) return null;

            Client? client = _context.Clients.FirstOrDefault(c => c.ExternalId.ToString() == dto.ClientExternalId);
            if (client == null) return null;

            Service? service = _context.Services.FirstOrDefault(s => s.ExternalId.ToString() == dto.ServiceExternalId);
            if (service == null) return null;

            Appointment appointment = new()
            {
                ExternalId = Guid.NewGuid(),
                BusinessId = business.Id,
                Business = business,
                ProfessionalId = professional.Id,
                Professional = professional,
                ClientId = client.Id,
                Client = client,
                ServiceId = service.Id,
                Service = service,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Notes = dto.Notes,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment?> GetByExternalId(Guid externalId)
        {
            Appointment? appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.ExternalId == externalId);
            return appointment;
        }
    }
}
