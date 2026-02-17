using turno_clave_API.Application.DTOs;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Data;

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
    }
}
