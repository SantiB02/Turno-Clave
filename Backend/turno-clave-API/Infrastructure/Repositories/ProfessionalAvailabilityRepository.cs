using Microsoft.EntityFrameworkCore;
using turno_clave_API.Application.DTOs.ProfessionalAvailability;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class ProfessionalAvailabilityRepository : IProfessionalAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public ProfessionalAvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<ProfessionalAvailability>> GetAvailabilitiesAsync()
        {
            return _context.ProfessionalAvailabilities.ToListAsync();
        }

        public Task<ProfessionalAvailability?> GetAvailabilityByExternalIdAsync(Guid externalId)
        {
            return _context.ProfessionalAvailabilities.Include(av => av.Professional).FirstOrDefaultAsync(a => a.ExternalId == externalId);
        }

        public void AddAvailability(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Add(availability);
        }

        public void UpdateAvailability(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Update(availability);
        }

        public async Task<List<ProfessionalAvailabilityDTO>> UpdateAvailabilitiesAsync(Professional professional, UpdateProfessionalAvailabilitiesDTO dto)
        {
            _context.ProfessionalAvailabilities.RemoveRange(professional.Availabilities);

            professional.Availabilities = dto.Availabilities
                .Select(a => new ProfessionalAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                })
                .ToList();

            await SaveAsync();

            return professional.Availabilities
                .Select(a => new ProfessionalAvailabilityDTO
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                })
                .ToList();
        }

        public async Task DeleteAvailabilityAsync(Guid availabilityId)
        {
            ProfessionalAvailability? availability = await _context.ProfessionalAvailabilities.FirstOrDefaultAsync(a => a.ExternalId == availabilityId) ?? throw new KeyNotFoundException($"Availability with ExternalId {availabilityId} not found.");
            _context.ProfessionalAvailabilities.Remove(availability);
        }

        public async Task DeleteAvailabilityAsync(ProfessionalAvailability availability)
        {
            _context.ProfessionalAvailabilities.Remove(availability);
        }

        public async Task<bool> IsAvailabilityTakenAsync(Professional professional, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.ProfessionalAvailabilities
                .Where(av => av.ProfessionalId == professional.Id)
                .AnyAsync(a =>
                    a.DayOfWeek == dayOfWeek &&
                    startTime < a.EndTime &&
                    endTime > a.StartTime
                );
        }

        public async Task<bool> IsDayWorkDayAsync(Professional professional, DayOfWeek dayOfWeek)
        {
            return await _context.ProfessionalAvailabilities
                .Where(av => av.ProfessionalId == professional.Id)
                .AnyAsync(a => a.DayOfWeek == dayOfWeek);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
