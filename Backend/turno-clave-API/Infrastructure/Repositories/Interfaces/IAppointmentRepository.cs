using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<List<Appointment>> GetAppointmentsAsync();
        public Task<Appointment?> GetAppointmentByExternalIdAsync(Guid externalId);
        public Task<List<Appointment>> GetByBusinessIdAndDateRangeAsync(int businessId, DateTimeOffset fromDate, DateTimeOffset toDate);
        public void AddAppointment(Appointment appointment);
        public void UpdateAppointment(Appointment appointment);
        public Task DeleteAppointmentAsync(Guid appointmentId);
        public Task<bool> IsAppointmentTakenAsync(int professionalId, DateTimeOffset startTime, DateTimeOffset endTime);
        public Task<bool> ExistsByReservationCodeAsync(string reservationCode);
        public Task SaveAsync();
    }
}
