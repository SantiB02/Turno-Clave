using System;
using System.Linq;
using turno_clave_API.Infrastructure.Time;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using turno_clave_API.Application.DTOs.Business;

namespace turno_clave_API.Application.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _context;

        public BusinessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Business> CreateAsync(CreateBusinessDTO dto)
        {
            string slug = GenerateSlug(dto.Name);

            // Validate/normalize timezone identifier
            string timezoneId = TimeZoneHelper.NormalizeTimeZoneId(dto.TimeZone);

            Business business = new()
            {
                Name = dto.Name,
                Slug = slug,
                Description = dto.Description,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                TimeZone = timezoneId,
            };

            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            return business;
        }

        

        public async Task<Business?> GetByExternalId(Guid externalId)
        {
            Business? business = await _context.Businesses.FirstOrDefaultAsync(b => b.ExternalId == externalId);
            return business;
        }

        public async Task<Business?> UpdateAsync(Guid externalId, UpdateBusinessDTO dto)
        {
            Business? business = await _context.Businesses.FirstOrDefaultAsync(b => b.ExternalId == externalId);
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

            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business?> DeleteAsync(Guid externalId)
        {
            Business? business = await _context.Businesses.FirstOrDefaultAsync(b => b.ExternalId == externalId);
            if (business != null)
            {
                business.IsActive = false;
                await _context.SaveChangesAsync();
            }
            return business;
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
