using turno_clave_API.Application.DTOs.Professional;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly ILogger _logger;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IBusinessRepository _businessRepository;

        public ProfessionalService(ILogger<ProfessionalService> logger, IProfessionalRepository professionalRepository, IBusinessRepository businessRepository)
        {
            _logger = logger;
            _professionalRepository = professionalRepository;
            _businessRepository = businessRepository;
        }

        public async Task<Professional> CreateAsync(CreateProfessionalDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                throw new KeyNotFoundException($"Business with ExternalId {dto.BusinessExternalId} not found.");

            Professional professional = new()
            {
                BusinessId = business.Id,
                Business = business,
                Name = dto.Name,
            };

            _professionalRepository.AddProfessional(professional);
            await _professionalRepository.SaveAsync();

            return professional;
        }

        public async Task<Result<IEnumerable<Professional>>> GetByBusinessExternalIdAsync(Guid businessExternalId)
        {
            IEnumerable<Professional> professionals = await _professionalRepository.GetProfessionalsByBusinessExternalIdAsync(businessExternalId);
            return Result<IEnumerable<Professional>>.Success(professionals);
        }

        public async Task<Professional?> GetByExternalIdAsync(Guid externalId)
        {
            Professional? professional = await _professionalRepository.GetProfessionalByExternalIdAsync(externalId);
            return professional;
        }

        public async Task<Professional?> UpdateAsync(UpdateProfessionalDTO dto)
        {
            Professional? professional = await _professionalRepository.GetProfessionalByExternalIdAsync(dto.ExternalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ExternalId {dto.ExternalId} not found.");

            professional.Name = dto.Name;
            _professionalRepository.UpdateProfessional(professional);
            await _professionalRepository.SaveAsync();
            return professional;
        }

        public async Task<Professional?> DeleteAsync(Guid externalId)
        {
            Professional? professional = await _professionalRepository.GetProfessionalByExternalIdAsync(externalId);
            if (professional != null)
            {
                professional.IsActive = false;
                await _professionalRepository.SaveAsync();
            } else
            {
                throw new KeyNotFoundException($"Professional with ExternalId {externalId} not found.");
            }

            return professional;
        }
    }
}
