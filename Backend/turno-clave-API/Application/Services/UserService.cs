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
            User user = new()
            {
                GoogleId = payload.Subject,
                Email = "santibrasca02@gmail.com",
                Name = "Santiago Brasca",
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
