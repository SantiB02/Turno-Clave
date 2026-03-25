using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Appointment>> GetAppointmentsAsync()
        {
            return _context.Appointments.ToListAsync();
        }

        public Task<Appointment?> GetAppointmentByExternalIdAsync(Guid externalId)
        {
            return _context.Appointments.Include(a => a.Business).Include(a => a.Professional).Include(a => a.Client).Include(a => a.Service).FirstOrDefaultAsync(a => a.ExternalId == externalId);
        }

        public void AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public async Task DeleteAppointmentAsync(Guid appointmentId)
        {
            Appointment? appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.ExternalId == appointmentId) ?? throw new KeyNotFoundException($"Appointment with ExternalId {appointmentId} not found."); // TODO: return null instead of throwing exception and handle it in service layer
            _context.Appointments.Remove(appointment);
        }

        public async Task<bool> IsAppointmentTakenAsync(int professionalId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            return await _context.Appointments
                .Where(a => a.ProfessionalId == professionalId)
                .AnyAsync(a =>
                    startTime < a.EndDateTime &&
                    endTime > a.StartDateTime
                );
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
