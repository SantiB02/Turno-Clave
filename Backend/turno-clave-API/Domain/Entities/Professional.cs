namespace turno_clave_API.Domain.Entities
{
    public class Professional
    {
        public int Id { get; set; }

        public int BusinessId { get; set; }
        public required Business Business { get; set; }

        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
