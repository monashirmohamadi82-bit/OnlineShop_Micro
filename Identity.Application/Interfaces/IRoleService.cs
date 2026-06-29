using IdentityGrpcService;
using System.Globalization;
using System.Security.Claims;

namespace IdentityGrcpService.C2_ApplicationIdentity.Interfaces
{
    public interface IRoleService 
    {

        Task<CreateRoleResponse> CreateRoleAsync(string RoleName);
        Task<List<string>> GetAllRolesAsync();
        Task<bool> DeleteRoleAsync(string RoleName);
        Task<bool> AddClaimToRoleAsync(string RoleName,string claimType,string claimValue);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleName);
        Task<bool> RemoveClaimFromRoleAsync(string roleName,string claimType,string claimValue);


        Task<bool> AddUserToRoleAsync(string UserName, string RoleName);
    }
}
