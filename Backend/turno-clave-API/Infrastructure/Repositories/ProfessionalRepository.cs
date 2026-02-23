using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;

namespace turno_clave_API.Infrastructure.Repositories
{
    public class ProfessionalRepository
    {
        private readonly AppDbContext _context;

        public ProfessionalRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Professional>> GetProfessionalsAsync()
        {
            return _context.Professionals.ToListAsync();
        }

        public Task<Professional?> GetProfessionalByExternalIdAsync(Guid externalId)
        {
            return _context.Professionals.FirstOrDefaultAsync(p => p.ExternalId == externalId);
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
