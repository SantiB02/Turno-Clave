using Google.Apis.Auth;
using turno_clave_API.Application.DTOs.User;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> CreateFromGooglePayloadAsync(GoogleJsonWebSignature.Payload payload);
        Task<User?> GetByExternalIdAsync(Guid externalId);
    }
}
