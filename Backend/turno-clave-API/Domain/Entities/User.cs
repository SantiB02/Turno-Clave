using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } 
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation properties

    }
}
