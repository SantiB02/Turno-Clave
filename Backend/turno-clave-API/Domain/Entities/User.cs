using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public string GoogleId { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<UserBusiness> UserBusinesses { get; set; } = [];
    }
}
