using IdentityGrcpService.Entites;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityGrcpService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        //add claim to user
        public async Task<bool> AddClaimToUserAsync(Guid UserId, string claimType, string claimValue)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (user == null)
                {
                    return false;
                }

                var claim = new Claim(claimType, claimValue);
                var result = await _userManager.AddClaimAsync(user, claim);

                return result.Succeeded;

            }
            catch (Exception ex)
            {
                throw new Exception("Add Claim To User failed: " + ex.Message);
            }
        }



        //get user claim
        public async Task<IList<Claim>> GetUserClaimsAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return new List<Claim>();
                }

                var claims = await _userManager.GetClaimsAsync(user);
                return claims.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Get User Claim failed: " + ex.Message);
            }
        }
        //get user role
        public async Task<List<string>> GetUserRole(Guid userId)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return new List<string>();
                }

                var roles = await _userManager.GetRolesAsync(user);

                var roleName = new List<string>();

                foreach (var role in roles)
                {
                    roleName.Add(role);
                }
                return roleName;

            }
            catch (Exception ex)
            {
                throw new Exception("Get User Roles failed: " + ex.Message);
            }
        }

        //remove user role
        public async Task<bool> RemoveUserRole(Guid userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return false;
                }

                var hasRole = await _userManager.IsInRoleAsync(user, roleName);

                if (!hasRole)
                {
                    return false;
                }

                var result = await _userManager.RemoveFromRoleAsync(user,roleName);
                
                return result.Succeeded;

            }
            catch (Exception ex)
            {
                throw new Exception("Remove User Role failed: " + ex.Message);
            }

        }


        ///remove claim from user

        public async Task<bool> RemoveClaimFromUserAsync(Guid userId, string claimType, string claimValue)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return false;
                }

                var calim = new Claim(claimType, claimValue);

                var result = await _userManager.RemoveClaimAsync(user, calim);

                return result.Succeeded;

            }
            catch (Exception ex)
            {
                throw new Exception("Remove Claim From User failed: " + ex.Message);
            }

        }


    }
}

