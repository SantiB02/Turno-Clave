namespace turno_clave_API.Application.DTOs.Service
{
    public class MinimalServiceDTO
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }

        internal static MinimalServiceDTO FromService(Domain.Entities.Service service)
        {
            return new MinimalServiceDTO
            {
                ExternalId = service.ExternalId,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes
            };
        }
    }
}
