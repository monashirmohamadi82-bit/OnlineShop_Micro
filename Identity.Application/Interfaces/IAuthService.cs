using IdentityGrpcService;

namespace IdentityGrcpService.C2_ApplicationIdentity.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
