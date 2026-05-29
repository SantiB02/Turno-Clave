
namespace turno_clave_API.Application.DTOs.Client
{
    public class ClientDTO
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Notes { get; set; }

        internal static ClientDTO FromClient(Domain.Entities.Client client)
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
