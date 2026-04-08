using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Domain.Entities
{
    public class UserBusiness
    {
        public int UserId { get; set; }
        public required User User { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public UserRole Role { get; set; } = UserRole.Owner;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
