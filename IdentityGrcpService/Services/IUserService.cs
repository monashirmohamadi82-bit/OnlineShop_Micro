using System.Security.Claims;

namespace IdentityGrcpService.Services
{
    public interface IUserService
    {
        Task<bool> AddClaimToUserAsync(Guid UserId, string claimType, string claimValue);
        Task<IList<Claim>> GetUserClaimsAsync(Guid userId);
        Task<List<string>> GetUserRole(Guid userId);
        Task<bool> RemoveUserRole(Guid userId, string roleName);
        Task<bool> RemoveClaimFromUserAsync(Guid userId, string claimType, string claimValue);

    }
}
