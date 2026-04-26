using System;
using System.Linq;
using turno_clave_API.Infrastructure.Time;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Infrastructure.Repositories.Interfaces;
using turno_clave_API.Common;

namespace turno_clave_API.Application.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BusinessService> _logger;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserService _userService;
        private readonly IBusinessAvailabilityRepository _businessAvailabilityRepository;


        public BusinessService(AppDbContext context, ILogger<BusinessService> logger, IBusinessRepository businessRepository, IUserService userService, IBusinessAvailabilityRepository businessAvailabilityRepository)
        {
            _context = context;
            _logger = logger;
            _businessRepository = businessRepository;
            _userService = userService;
            _businessAvailabilityRepository = businessAvailabilityRepository;
        }

        public async Task<Result<Business>> CreateAsync(CreateBusinessDTO dto, Guid userExternalId)
        {
            string baseSlug = GenerateSlug(dto.Name);
            string slug = baseSlug;
            int counter = 2;

            // Adds a number if the slug already exists, to ensure uniqueness (e.g. "my-business", "my-business-2", "my-business-3", etc.)
            while (await _businessRepository.SlugExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            Result<string> timeZoneResult = TimeZoneHelper.NormalizeTimeZoneId(dto.TimeZone);

            if (!timeZoneResult.IsSuccess)
            {
                return Result<Business>.Failure($"Invalid time zone: {dto.TimeZone}");
            }

            Business business = new()
            {
                Name = dto.Name,
                Slug = slug,
                Description = dto.Description ?? string.Empty,
                LogoUrl = dto.LogoUrl ?? string.Empty,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                TimeZone = timeZoneResult.Value!, // null-forgiving because we know it's not null if IsSuccess is true
                BusinessAvailabilities = dto.Availabilities?.Select(a => new BusinessAvailability
                {
                    Day = a.Day,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList() ?? []
            };

            User? user = await _userService.GetByExternalIdAsync(userExternalId);

            if (user == null)
            {
                return Result<Business>.Failure($"Unauthorized");
            }

            UserBusiness userBusiness = new()
            {
                User = user,
                Business = business,
            };

            _context.Businesses.Add(business);
            _context.UserBusinesses.Add(userBusiness);


            await _context.SaveChangesAsync();

            return Result<Business>.Success(business); // TODO: Map to a DTO instead of returning the entity directly (to avoid cycles and sensitive data exposure)
        }

        public async Task<Business?> GetByExternalIdAsync(Guid externalId)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(externalId);
            return business;
        }

        public async Task<IEnumerable<Business>> GetByUserExternalIdAsync(Guid userExternalId)
        {
            IEnumerable<Business> businesses = await _businessRepository.GetBusinessesByUserExternalIdAsync(userExternalId);
            return businesses;
        }

        public async Task<Business?> UpdateAsync(UpdateBusinessDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.ExternalId);
            if (business == null)
            {
                return null;
            }

            business.Name = dto.Name;
            business.Description = dto.Description;
            business.Phone = dto.Phone;
            business.Address = dto.Address;
            business.City = dto.City;
            business.Country = dto.Country;

            await _businessRepository.SaveAsync();
            return business;
        }

        public async Task<Business?> DeleteAsync(Guid externalId)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(externalId);
            if (business != null)
            {
                business.IsActive = false;
                await _businessRepository.SaveAsync();
            }
            return business;
        }

        // Business availability implementations
        public async Task<IEnumerable<BusinessAvailabilityDTO>> GetGlobalAvailabilityAsync(Guid businessExternalId)
        {
            var list = await _businessAvailabilityRepository.GetByBusinessExternalIdAsync(businessExternalId);
            return list.Select(b => new BusinessAvailabilityDTO
            {
                ExternalId = b.ExternalId,
                Day = b.Day,
                StartTime = b.StartTime,
                EndTime = b.EndTime
            });
        }

        public async Task<BusinessAvailabilityDTO> CreateGlobalAvailabilityAsync(Guid businessExternalId, CreateBusinessAvailabilityDTO dto)
        {
            // Find business
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(businessExternalId);
            if (business == null) throw new KeyNotFoundException($"Business {businessExternalId} not found");
            // Validate DTO (should have been validated by model binding, but double-check)
            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            // Check for overlapping availabilities for the same business/day
            var existing = (await _businessAvailabilityRepository.GetByBusinessExternalIdAsync(businessExternalId))
                .Where(x => x.Day == dto.Day && x.IsActive);

            bool overlaps = existing.Any(e => !(dto.EndTime <= e.StartTime || dto.StartTime >= e.EndTime));
            if (overlaps)
                throw new InvalidOperationException("The provided availability overlaps an existing one.");

            var entity = new BusinessAvailability
            {
                Business = business,
                BusinessId = business.Id,
                Day = dto.Day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _businessAvailabilityRepository.Add(entity);
            await _businessAvailabilityRepository.SaveAsync();

            return new BusinessAvailabilityDTO
            {
                ExternalId = entity.ExternalId,
                Day = entity.Day,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };
        }

        public async Task<BusinessAvailabilityDTO?> UpdateGlobalAvailabilityAsync(BusinessAvailabilityDTO dto)
        {
            var entity = await _businessAvailabilityRepository.GetByExternalIdAsync(dto.ExternalId);
            if (entity == null) return null;
            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            // Check overlap against other availabilities
            var existing = (await _businessAvailabilityRepository.GetByBusinessExternalIdAsync(entity.Business.ExternalId))
                .Where(x => x.Id != entity.Id && x.Day == dto.Day && x.IsActive);

            bool overlaps = existing.Any(e => !(dto.EndTime <= e.StartTime || dto.StartTime >= e.EndTime));
            if (overlaps)
                throw new InvalidOperationException("The provided availability overlaps an existing one.");

            entity.Day = dto.Day;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;

            _businessAvailabilityRepository.Update(entity);
            await _businessAvailabilityRepository.SaveAsync();

            return new BusinessAvailabilityDTO
            {
                ExternalId = entity.ExternalId,
                Day = entity.Day,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };
        }

        public async Task<bool> DeleteGlobalAvailabilityAsync(Guid externalId)
        {
            await _businessAvailabilityRepository.DeleteAsync(externalId);
            await _businessAvailabilityRepository.SaveAsync();
            return true;
        }

        private static string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            // Normalize and remove diacritics (NFD -> remove NonSpacingMark -> NFC)
            var normalized = name.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var ch in normalized)
            {
                var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            var noDiacritics = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);

            // Lowercase, replace whitespace with hyphens, remove invalid chars, collapse hyphens, trim
            string slug = noDiacritics.ToLowerInvariant().Trim();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-").Trim('-');

            // Limit length to 80 characters
            if (slug.Length > 80)
                slug = slug.Substring(0, 80);

            // Fallback to a short random identifier if slug becomes empty
            if (string.IsNullOrEmpty(slug))
                slug = Guid.NewGuid().ToString("n").Substring(0, 8);

            return slug;
        }
    }
}
