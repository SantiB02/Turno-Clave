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
using turno_clave_API.Application.DTOs.BusinessAvailability;

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

        public async Task<Result<MinimalBusinessDTO>> CreateAsync(CreateBusinessDTO dto, Guid userExternalId)
        {
            User? user = await _userService.GetByExternalIdAsync(userExternalId);

            if (user == null)
            {
                return Result<MinimalBusinessDTO>.Failure($"Unauthorized");
            }

            string slug = await GenerateSlugAsync(dto.Name);
            

            Result<string> timeZoneResult = TimeZoneHelper.NormalizeTimeZoneId(dto.TimeZone);

            if (!timeZoneResult.IsSuccess)
            {
                return Result<MinimalBusinessDTO>.Failure($"Invalid time zone: {dto.TimeZone}");
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
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList() ?? []
            };


            if (user.ActiveBusinessExternalId == null)
            {
                user.ActiveBusinessExternalId = business.ExternalId;
            }

            UserBusiness userBusiness = new()
            {
                User = user,
                Business = business,
            };

            Professional defaultProfessional = new()
            {
                Name = user.Name,
                Business = business,
                Availabilities = business.BusinessAvailabilities.Select(static a => new ProfessionalAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = TimeOnly.FromTimeSpan(a.StartTime),
                    EndTime = TimeOnly.FromTimeSpan(a.EndTime),
                }).ToList()
            };

            business.Professionals.Add(defaultProfessional);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Businesses.Add(business);
                _context.UserBusinesses.Add(userBusiness);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result<MinimalBusinessDTO>.Success(Business.ToDto(business));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating business");
                await transaction.RollbackAsync();
                return Result<MinimalBusinessDTO>.Failure("An error occurred while creating the business.");
            }
        }

        public async Task<BusinessDetailDTO?> GetByExternalIdAsync(Guid externalId)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(externalId);
            return business != null ? Business.ToDetailDto(business) : null;
        }

        public async Task<IEnumerable<BusinessDetailDTO>> GetByUserExternalIdAsync(Guid userExternalId)
        {
            IEnumerable<Business> businesses = await _businessRepository.GetBusinessesByUserExternalIdAsync(userExternalId);
            return businesses.Select(Business.ToDetailDto);
        }

        public async Task<Result<MinimalBusinessDTO?>> UpdateAsync(Guid externalId, UpdateBusinessDTO dto)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(externalId);
            if (business == null)
            {
                return Result<MinimalBusinessDTO?>.Failure($"BUSINESS_NOT_FOUND");
            }

            // Change slug only if business name is different from current one
            if (!business.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                string newSlug = await GenerateSlugAsync(dto.Name);
                business.Slug = newSlug;
            }

            // Change the business' time zone only if the country changed
            if (!business.Country.Equals(dto.Country, StringComparison.OrdinalIgnoreCase))
            {
                Result<string> timeZoneResult = TimeZoneHelper.NormalizeTimeZoneId(dto.TimeZone);

                if (!timeZoneResult.IsSuccess)
                {
                    return Result<MinimalBusinessDTO?>.Failure($"INVALID_TIMEZONE");
                }
                business.TimeZone = dto.TimeZone;
            }

            business.Name = dto.Name;
            business.Description = dto.Description;
            business.PaymentMethods = dto.PaymentMethods;
            business.Phone = dto.Phone;
            business.Country = dto.Country;
            business.State = dto.State;
            business.City = dto.City;
            business.Address = dto.Address;

            await _businessRepository.SaveAsync();
            return Result<MinimalBusinessDTO?>.Success(Business.ToDto(business));
        }

        public async Task<MinimalBusinessDTO?> DeleteAsync(Guid externalId)
        {
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(externalId);
            if (business != null)
            {
                business.IsActive = false;
                await _businessRepository.SaveAsync();
                return Business.ToDto(business);
            } else
            {
                return null;
            }
        }

        // Business availability implementations
        public async Task<IEnumerable<BusinessAvailabilityDTO>> GetGlobalAvailabilityAsync(Guid businessExternalId)
        {
            var list = await _businessAvailabilityRepository.GetByBusinessExternalIdAsync(businessExternalId);
            return list.Select(b => new BusinessAvailabilityDTO
            {
                ExternalId = b.ExternalId,
                DayOfWeek = b.DayOfWeek,
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
                .Where(x => x.DayOfWeek == dto.DayOfWeek && x.IsActive);

            bool overlaps = existing.Any(e => !(dto.EndTime <= e.StartTime || dto.StartTime >= e.EndTime));
            if (overlaps)
                throw new InvalidOperationException("The provided availability overlaps an existing one.");

            var entity = new BusinessAvailability
            {
                Business = business,
                BusinessId = business.Id,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _businessAvailabilityRepository.Add(entity);
            await _businessAvailabilityRepository.SaveAsync();

            return new BusinessAvailabilityDTO
            {
                ExternalId = entity.ExternalId,
                DayOfWeek = entity.DayOfWeek,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };
        }

        public async Task<BusinessAvailabilityDTO?> UpdateGlobalAvailabilityAsync(Guid externalId, UpdateBusinessAvailabilityDTO dto)
        {
            var entity = await _businessAvailabilityRepository.GetByExternalIdAsync(externalId);
            if (entity == null) return null;
            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            // Check overlap against other availabilities
            var existing = (await _businessAvailabilityRepository.GetByBusinessExternalIdAsync(entity.Business.ExternalId))
                .Where(x => x.Id != entity.Id && x.DayOfWeek == dto.DayOfWeek && x.IsActive);

            bool overlaps = existing.Any(e => !(dto.EndTime <= e.StartTime || dto.StartTime >= e.EndTime));
            if (overlaps)
                throw new InvalidOperationException("The provided availability overlaps an existing one.");

            entity.DayOfWeek = dto.DayOfWeek;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;

            _businessAvailabilityRepository.Update(entity);
            await _businessAvailabilityRepository.SaveAsync();

            return new BusinessAvailabilityDTO
            {
                ExternalId = entity.ExternalId,
                DayOfWeek = entity.DayOfWeek,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };
        }

        public async Task<List<BusinessAvailabilityDTO>?> UpdateGlobalAvailabilitiesAsync(Guid businessExternalId, UpdateBusinessAvailabilitiesDTO dto)
        {
            Business? business = await _businessRepository
                .GetBusinessByExternalIdAsync(businessExternalId);

            if (business == null) return null;

            _context.BusinessAvailabilities.RemoveRange(business.BusinessAvailabilities);

            business.BusinessAvailabilities = dto.Availabilities
                .Select(a => new BusinessAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                })
                .ToList();

            await _context.SaveChangesAsync();

            return business.BusinessAvailabilities
                .Select(a => new BusinessAvailabilityDTO
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                })
                .ToList();
        }

        public async Task<bool> DeleteGlobalAvailabilityAsync(Guid externalId)
        {
            await _businessAvailabilityRepository.DeleteAsync(externalId);
            await _businessAvailabilityRepository.SaveAsync();
            return true;
        }

        private async Task<string> GenerateSlugAsync(string name)
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
            string baseSlug = noDiacritics.ToLowerInvariant().Trim();
            baseSlug = System.Text.RegularExpressions.Regex.Replace(baseSlug, @"\s+", "-");
            baseSlug = System.Text.RegularExpressions.Regex.Replace(baseSlug, @"[^a-z0-9\-]", "");
            baseSlug = System.Text.RegularExpressions.Regex.Replace(baseSlug, @"-+", "-").Trim('-');

            // Limit length to 80 characters
            if (baseSlug.Length > 80)
                baseSlug = baseSlug.Substring(0, 80);

            // Fallback to a short random identifier if slug becomes empty
            if (string.IsNullOrEmpty(baseSlug))
                baseSlug = Guid.NewGuid().ToString("n").Substring(0, 8);

            string slug = baseSlug;
            int counter = 2;

            // Adds a number if the slug already exists, to ensure uniqueness (e.g. "my-business", "my-business-2", "my-business-3", etc.)
            while (await _businessRepository.SlugExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
    }
}
