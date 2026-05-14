using Microsoft.EntityFrameworkCore;
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

        public Task<IEnumerable<Professional>> GetProfessionalsByBusinessExternalIdAsync(Guid businessExternalId)
        {
            return _context.Professionals
                .Include(p => p.Availabilities)
                .Include(p => p.Business)
                .Where(p => p.Business.ExternalId == businessExternalId)
                .ToListAsync()
                .ContinueWith(t => t.Result.AsEnumerable());
        }

        public Task<Professional?> GetProfessionalByExternalIdAsync(Guid externalId)
        {
            return _context.Professionals
                .Include(p => p.Availabilities)
                .Include(p => p.Business)
                    .ThenInclude(b => b.BusinessAvailabilities)
                .FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task<List<Professional>> GetProfessionalsByExternalIdsAsync(List<Guid> externalIds)
        {
            if (externalIds.Count == 0)
            {
                return [];
            }

            return await _context.Professionals.Where(p => externalIds.Contains(p.ExternalId)).ToListAsync();
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
