namespace turno_clave_API.Application.DTOs.User
{
    public class CreateUserDTO
    {
        public required string GoogleId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
