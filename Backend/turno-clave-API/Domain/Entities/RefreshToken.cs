namespace turno_clave_API.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid UserExternalId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => RevokedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;
    }
}
