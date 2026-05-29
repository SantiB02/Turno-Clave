using turno_clave_API.Application.DTOs.Client;

namespace turno_clave_API.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; } = Guid.NewGuid();

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Notes { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<Appointment> Appointments { get; set; } = [];

        // We don't need to show the whole Business. Only its main identifying data
        public static ClientDTO ToDto (Client client)
        {
            return new ClientDTO
            {
                ExternalId = client.ExternalId,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Notes = client.Notes
            };
        }
    }
}
