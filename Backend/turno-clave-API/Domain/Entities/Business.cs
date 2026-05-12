using turno_clave_API.Application.DTOs.Business;
using turno_clave_API.Domain.Enums;

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
        public List<PaymentMethod> PaymentMethods { get; set; } = [];
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
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Client> Clients { get; set; } = new List<Client>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<AvailabilityException> AvailabilityExceptions { get; set; } = new List<AvailabilityException>();
        public ICollection<Professional> Professionals { get; set; } = new List<Professional>();
        public ICollection<UserBusiness> UserBusinesses { get; set; } = new List<UserBusiness>();

        // Global weekly availability template for the business. This is a business-level schedule
        // that the frontend can show during onboarding. Professionals can still have their own
        // availability which will be applied/merged according to business rules (e.g. intersect or override).
        public ICollection<BusinessAvailability> BusinessAvailabilities { get; set; } = new List<BusinessAvailability>();

        public static MinimalBusinessDTO ToDto(Business business)
        {
            return new MinimalBusinessDTO
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
                PaymentMethods = business.PaymentMethods,
                Address = business.Address,
                City = business.City,
                State = business.State,
                Country = business.Country,

                Availabilities = business.BusinessAvailabilities.Select(ba => new BusinessAvailabilityDTO
                {
                    ExternalId = ba.ExternalId,
                    Day = ba.Day,
                    StartTime = ba.StartTime,
                    EndTime = ba.EndTime,
                }).ToList(),
            };
        }
    }
}
