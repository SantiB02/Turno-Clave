using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Task<User> GetCurrentUserAsync();
        Task<Guid> GetActiveBusinessExternalIdAsync();
        Guid GetExternalId();
    }
}
