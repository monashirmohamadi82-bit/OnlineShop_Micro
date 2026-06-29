using IdentityGrcpService.C2_ApplicationIdentity.Interfaces;
using IdentityGrcpService.C3_InfrastructureIdentity.Services;
using IdentityGrpcService;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityGrcpService.C3_InfrastructureIdentity.Services
{
    public class RoleService : IRoleService
    {

        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;


        public RoleService(RoleManager<ApplicationRole> role, UserManager<ApplicationUser> _userManager)
        {
            roleManager = role;
            userManager = _userManager;
        }

       


        ///create  role 
        public async Task<bool> CreateRoleAsync(string RoleName)
        {

            try {
                ApplicationRole role = new ApplicationRole();

                role.Name = RoleName;
                var result = await roleManager.CreateAsync(role);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception("Create Role failed: " + ex.Message);
            }
         }



        // get roles
        public async Task<List<string>> GetAllRolesAsync()
        {
            try
            {
                var roles = roleManager.Roles.ToList();

                var roleName=new List<string>();

                foreach(var role in roles)
                {
                    roleName.Add(role.Name);
                }


                return roleName;

            }
            catch (Exception ex)
            {
                throw new Exception("Get All Role failed: " + ex.Message);
            }

        }


        //delete role 
        public async Task<bool> DeleteRoleAsync(string RoleName)
        {
            try
            {
                var role= await roleManager.FindByNameAsync(RoleName);
                if(role == null)
                {
                    return false;
                }
                var result=await roleManager.DeleteAsync(role);
                return result.Succeeded;

            }
            catch (Exception ex)
            {
                throw new Exception("Delete Role failed: " + ex.Message);
            }

        }









        //asing user to role 
        public  async Task<bool> AddUserToRoleAsync(string UserName, string RoleName)
        {
            try
            {
                var user=await userManager.FindByNameAsync(UserName);
                if(user == null) {
                    return false;
                }



                var roleExists = await roleManager.RoleExistsAsync(RoleName);
                if(roleExists == false) {
                    return false;
                }

                var result= await userManager.AddToRoleAsync(user, RoleName);

                return result.Succeeded;

            }
            catch (Exception ex)
            {
                throw new Exception("Asing User To  Role failed: " + ex.Message);
            }
        }



        //assing claim to role
        public async Task<bool> AddClaimToRoleAsync(string RoleName, string claimType, string claimValue)
        {
            try
            {
                var role=await roleManager.FindByNameAsync(RoleName);
                if(role == null)
                {
                    return false;

                 }

                var claim= new Claim(claimType, claimValue);
                 var result=await roleManager.AddClaimAsync(role, claim);
                return result.Succeeded;
            
            }
            catch (Exception ex)
            {
                throw new Exception("Asing Claim To  Role failed: " + ex.Message);
            }

        }


        //get role claims
        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleName)
        {

            try
            {
                var role=await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new List<Claim>();
                }

                var claims = await roleManager.GetClaimsAsync(role);
                return claims;

            }
            catch (Exception ex)
            {
                throw new Exception("Get role Claims failed: " + ex.Message);
            }
        }

        //remove claim from role
        public async Task<bool> RemoveClaimFromRoleAsync( string roleName, string claimType,string claimValue)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    return false;
                }

                var claim = new Claim(claimType, claimValue);

                var result = await roleManager.RemoveClaimAsync(role, claim);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception("Remove Claim From Role failed: " + ex.Message);
            }
        }
    }
}
