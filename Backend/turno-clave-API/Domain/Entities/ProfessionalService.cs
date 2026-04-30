using System.ComponentModel.DataAnnotations.Schema;

namespace turno_clave_API.Domain.Entities
{
    public class ProfessionalService
    {
        public int ProfessionalId { get; set; }
        public Professional Professional { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
