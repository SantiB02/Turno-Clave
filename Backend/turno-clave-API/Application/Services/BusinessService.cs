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

        public async Task<Business> CreateAsync(CreateBusinessDto dto)
        {
            Business business = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
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

        public async Task<Business?> UpdateAsync(Guid externalId, UpdateBusinessDto dto)
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
    }
}
