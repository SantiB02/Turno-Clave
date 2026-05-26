using Microsoft.EntityFrameworkCore;
using turno_clave_API.Application.DTOs.Availability;
using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.DTOs.Service;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly AppDbContext _context;

        public ProfessionalRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Professional>> GetProfessionalsAsync()
        {
            return _context.Professionals.Include(p => p.Business).ToListAsync();
        }

        public Task<List<Professional>> GetProfessionalsByBusinessExternalIdAsync(Guid businessExternalId)
        {
            return _context.Professionals
                .Include(p => p.Availabilities)
                .Include(p => p.Business)
                .Include(p => p.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
                .Where(p => p.Business.ExternalId == businessExternalId)
                .ToListAsync();
        }

        public async Task<List<ProfessionalDTO>> GetProfessionalDtosByBusinessExternalIdAsync(Guid businessExternalId)
        {
            return await _context.Professionals
                .Where(p => p.Business.ExternalId == businessExternalId)
                .Select(p => new ProfessionalDTO
                {
                    ExternalId = p.ExternalId,
                    BusinessExternalId = p.Business.ExternalId,
                    BusinessName = p.Business.Name,
                    Name = p.Name,
                    IsActive = p.IsActive,

                    Availabilities = p.Availabilities
                        .Select(pa => new NestedProfessionalAvailabilityDTO
                        {
                            ExternalId = pa.ExternalId,
                            DayOfWeek = pa.DayOfWeek,
                            StartTime = pa.StartTime,
                            EndTime = pa.EndTime,
                        })
                        .ToList(),

                    Services = p.ProfessionalServices
                        .Select(ps => new MinimalServiceDTO
                        {
                            ExternalId = ps.Service.ExternalId,
                            Name = ps.Service.Name,
                            Description = ps.Service.Description ?? "",
                            Price = ps.Service.Price,
                            DurationMinutes = ps.Service.DurationMinutes
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public Task<Professional?> GetProfessionalByExternalIdAsync(Guid externalId)
        {
            return _context.Professionals
                .Include(p => p.Availabilities)
                .Include(p => p.Business)
                    .ThenInclude(b => b.BusinessAvailabilities)
                .FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public Task<Professional?> GetProfessionalByExternalIdWithServicesAsync(Guid externalId)
        {
            return _context.Professionals
                .Include(p => p.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
                .FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task<List<Professional>> GetProfessionalsByExternalIdsAsync(List<Guid> externalIds)
        {
            if (externalIds == null || externalIds.Count == 0)
            {
                return new List<Professional>();
            }

            return await _context.Professionals
                .Include(p => p.Availabilities)
                .Include(p => p.ProfessionalServices)
                    .ThenInclude(ps => ps.Service)
                .Include(p => p.Business)
                .Where(p => externalIds.Contains(p.ExternalId))
                .ToListAsync();
        }

        public void AddProfessional(Professional professional)
        {
            _context.Professionals.Add(professional);
        }

        public void UpdateProfessional(Professional professional)
        {
            _context.Professionals.Update(professional);
        }

        public async Task DeleteProfessionalAsync(Guid professionalId)
        {
            Professional? professional = await GetProfessionalByExternalIdAsync(professionalId) ?? throw new KeyNotFoundException($"Professional with ExternalId {professionalId} not found.");
            _context.Professionals.Remove(professional);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
