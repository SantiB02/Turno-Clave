namespace turno_clave_API.Domain.Entities
{
    public class Business
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public string TimeZone { get; set; } = TimeZoneInfo.Local.Id; // Default to local time zone

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Service> Services { get; set; } = [];
        public ICollection<Client> Clients { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<Availability> Availabilities { get; set; } = [];
         public ICollection<AvailabilityException> AvailabilityExceptions { get; set; } = [];
    }
}
