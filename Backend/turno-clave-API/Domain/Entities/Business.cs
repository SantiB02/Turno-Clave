using turno_clave_API.Application.DTOs.Business;

namespace turno_clave_API.Domain.Entities
{
    public class Business
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Slug { get; set; } // URL-friendly identifier, e.g., "my-business-name"
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Country { get; set; }
        // Store IANA/Windows compatible timezone id. Default to UTC to avoid server-local timezone leaks.
        public string TimeZone { get; set; } = TimeZoneInfo.Utc.Id; // Default to UTC

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<Service> Services { get; set; } = [];
        public ICollection<Client> Clients { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<AvailabilityException> AvailabilityExceptions { get; set; } = [];
        public ICollection<Professional> Professionals { get; set; } = [];
        public ICollection<UserBusiness> UserBusinesses { get; set; } = [];

        public static BusinessDTO ToDto(Business business)
        {
            return new BusinessDTO
            {
                ExternalId = business.ExternalId,
                Name = business.Name,
                Slug = business.Slug
            };
        }

        public static BusinessDetailDTO ToDetailDto(Business business)
        {
            return new BusinessDetailDTO
            {
                ExternalId = business.ExternalId,
                Name = business.Name,
                Slug = business.Slug,
                Description = business.Description ?? string.Empty,
                LogoUrl = business.LogoUrl ?? string.Empty,
                Email = business.Email,
                Phone = business.Phone,
                Address = business.Address,
                City = business.City,
                State = business.State,
                Country = business.Country
            };
        }
    }
}
