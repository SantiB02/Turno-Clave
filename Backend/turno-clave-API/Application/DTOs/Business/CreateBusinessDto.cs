namespace turno_clave_API.Application.DTOs.Business
{
    public class CreateBusinessDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        // Time zone identifier for the business (IANA or Windows). Example: "America/Argentina/Buenos_Aires" or "UTC"
        public required string TimeZone { get; set; }
    }
}
