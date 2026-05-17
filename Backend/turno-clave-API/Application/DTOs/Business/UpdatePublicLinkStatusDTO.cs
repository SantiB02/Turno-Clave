using System.ComponentModel.DataAnnotations;

namespace turno_clave_API.Application.DTOs.Business
{
    public class UpdatePublicLinkStatusDTO
    {
        [Required]
        public bool IsPublicLinkEnabled { get; set; }
    }
}
