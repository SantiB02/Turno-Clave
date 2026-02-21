using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Repositories.Interfaces
{
    public interface IBusinessRepository : IDisposable
    {
        IEnumerable<Business> GetBusinesses();
        Business? GetBusinessByExternalId(Guid externalId);
        Business? GetBusinessByExternalId(string externalId);
        void AddBusiness(Business business);
        void UpdateBusiness(Business business);
        void DeleteBusiness(Business business);
        void Save();
    }
}
