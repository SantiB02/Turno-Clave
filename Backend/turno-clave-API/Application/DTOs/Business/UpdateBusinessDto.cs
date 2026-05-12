using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Application.DTOs.Business
{
    public class UpdateBusinessDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required List<PaymentMethod> PaymentMethods { get; set; }
        public required string Phone { get; set; }
        public required string Country { get; set; }
        public required string State { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        // Time zone identifier for the business (IANA or Windows). Example: "America/Argentina/Buenos_Aires" or "UTC"
        public required string TimeZone { get; set; }
    }
}
