using Google.Apis.Auth;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Common;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<User>> CreateFromGooglePayloadAsync(GoogleJsonWebSignature.Payload payload)
        {
            if (String.IsNullOrEmpty(payload.Subject) || String.IsNullOrEmpty(payload.Email) || String.IsNullOrEmpty(payload.Name))
            {
                return Result<User>.Failure("Invalid Google payload");
            }

            User user = new()
            {
                GoogleId = payload.Subject,
                Email = payload.Email,
                Name = payload.Name,
            };

            _userRepository.Add(user);
            await _userRepository.SaveAsync();
            return Result<User>.Success(user);
        }

        public Task<User?> GetByExternalIdAsync(Guid externalId)
        {
            return _userRepository.GetByExternalIdAsync(externalId);
        }
    }
}
