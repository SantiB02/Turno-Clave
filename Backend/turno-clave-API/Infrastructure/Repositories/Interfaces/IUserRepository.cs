using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetByExternalIdAsync(Guid externalId);
        public void Add(User user);
        public Task SaveAsync();
    }
}
