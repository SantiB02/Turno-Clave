using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Infrastructure.Repositories.Interfaces;

namespace turno_clave_API.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        private User? _cachedUser;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (_cachedUser != null)
            {
                return _cachedUser;
            }
            Guid userExternalId = GetExternalId();
            if (userExternalId == Guid.Empty)
            {
                throw new InvalidOperationException("No user ID claim found in the current context.");
            }

            User? user = await _userRepository.GetByExternalIdAsync(userExternalId);
            if (user == null)
            {
                throw new InvalidOperationException($"No user found with ExternalId {userExternalId}.");
            }
            _cachedUser = user;
            return user;
        }

        public async Task<Guid> GetActiveBusinessExternalIdAsync()
        {
            User user = await GetCurrentUserAsync();
            if (user.ActiveBusinessExternalId == null)
            {
                throw new InvalidOperationException("The current user does not have an active business.");
            }
            return user.ActiveBusinessExternalId.Value;
        }

        public Guid GetExternalId()
        {
            string? userExternalId = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            return userExternalId != null ? Guid.Parse(userExternalId) : Guid.Empty;
        }
    }
}
