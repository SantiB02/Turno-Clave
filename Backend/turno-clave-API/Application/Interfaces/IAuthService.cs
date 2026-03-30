namespace turno_clave_API.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginWithGoogle(string idToken);
    }
}
