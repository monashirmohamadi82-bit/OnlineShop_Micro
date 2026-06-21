using Azure.Core;
using Google.Protobuf;
using Grpc.Core;
using IdentityGrcpService;
using IdentityGrcpService.Entites;
using IdentityGrpcService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
namespace IdentityGrcpService.Services
{
    public class AuthSeviceIpml : AuthService.AuthServiceBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService jwtTokenService;
        private readonly IRoleService roleService;
        private readonly IUserService userService;
        public AuthSeviceIpml(UserManager<ApplicationUser> userManager, JwtTokenService _jwtTokenService,
           IRoleService _roleService, IUserService _userService)
        {
            _userManager = userManager;
            jwtTokenService = _jwtTokenService;
            roleService = _roleService;
            userService = _userService;
        }


        /// craete User in database
        public override async Task<RegisterResponse> Register(RegisterRequest registerRequest,
            ServerCallContext context
            )
        {
            try
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = registerRequest.UserName;
                user.Email = registerRequest.Email;

                var result = await _userManager.CreateAsync(user, registerRequest.Password);


                if (result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        Success = true,
                        Message = "User registered successfully."
                    };

                }


                // errors in reqest check 
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + " ";
                }

                return new RegisterResponse
                {
                    Success = false,
                    Message = errors
                };


            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }




        ///Login user in database

        public override async Task<LoginResponse> Login(LoginRequest loginRequest,
            ServerCallContext context)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginRequest.UserName);

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }



                var IsvalidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (!IsvalidPassword)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid password"
                    };

                }


                return new LoginResponse
                {
                    Success = true,
                    Token = await jwtTokenService.GenerateToken(user),
                    Message = "Login successful"
                };

            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }




        //create role 

        public override async Task<CreateRoleResponse> CreateRole(CreateRoleRequest createRoleRequest,
            ServerCallContext serverCallContext)
        {
            try
            {
                var result = await roleService.CreateRoleAsync(createRoleRequest.RoleName);

                return new CreateRoleResponse
                {
                    Success = result,
                    Message = result ? "Create Role successfully" : "Failed to Create role"

                };


            }
            catch (Exception ex)
            {
                return new CreateRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }





        //get all role

        public override async Task<GetAllRolesResponse> GetAllRoles(GetAllRolesRequest getAllRolesRequest,
            ServerCallContext serverCallContext)
        {
            try
            {
                var roles = await roleService.GetAllRolesAsync();
                var result = new GetAllRolesResponse();
                result.Roles.Add(roles);

                return result;

            }
            catch (Exception ex)
            {
                return new GetAllRolesResponse();

            }

        }



        //delete role
        public override async Task<DeleteRoleResponse> DeleteRole(DeleteRoleRequest deleteRoleRequest,
            ServerCallContext serverCallContext)
        {
            try
            {
                var result = await roleService.DeleteRoleAsync(deleteRoleRequest.RoleName);

                return new DeleteRoleResponse
                {
                    Success = result,
                    Message = result ? "Role deleted successfully" : "Failed to delete role"
                };

            }
            catch (Exception ex)
            {
                return new DeleteRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }


        //assing user to role
        public override async Task<AssignRoleResponse> AssignRoleToUser(AssignRoleRequest assignRoleRequest,
            ServerCallContext serverCallContext
            )
        {
            try
            {
                var result = await roleService.AddUserToRoleAsync(assignRoleRequest.UserName, assignRoleRequest.RoleName);

                return new AssignRoleResponse
                {
                    Success = result,
                    Message = result ? "Role assigned successfully" : "Failed to assign role"
                };


            }
            catch (Exception ex)
            {
                return new AssignRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }


        //get user role
        public override async Task<GetUserRoleResponse> GetUserRole(GetUserRoleRequset getUserRoleRequset,
           ServerCallContext serverCallContext
           )
        {
            try
            {
                var userId = Guid.Parse(getUserRoleRequset.UserId);
                var roles = await userService.GetUserRole(userId);
                var result = new GetUserRoleResponse();
                result.Roles.AddRange(roles);

                return result;

            }
            catch (Exception ex)
            {
                return new GetUserRoleResponse();
              
            }

        }


        //remove user role
        public override async Task<RemoveUserRoleResponse> RemoveUserRole(RemoveUserRoleRequset removeUserRoleRequset,
          ServerCallContext serverCallContext)
        {
            try
            {
                var userId = Guid.Parse(removeUserRoleRequset.UserId);
                var result = await userService.RemoveUserRole(userId, removeUserRoleRequset.RoleName);


                return new RemoveUserRoleResponse
                {
                    Success = result,
                    Message = result ? "Remove User Role successfully" : "Failed to remove user role"

                };
            }
            catch (Exception ex)
            {
                return new RemoveUserRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };

            }

        }





        ///add claim to user
        ///
        public override async Task<AddClaimToUserResponse> AddClaimToUser(AddClaimToUserRequest addClaimToUserRequest,
         ServerCallContext serverCallContext
         )
        {
            try
            {
                var userId = Guid.Parse(addClaimToUserRequest.UserId);

                var result = await userService.AddClaimToUserAsync(userId, addClaimToUserRequest.ClaimType, addClaimToUserRequest.ClaimValue);


                return new AddClaimToUserResponse
                {
                    Success = result,
                    Message = result ? "Claim added successfully" : "Failed to add claim"
                };

            }
            catch (Exception ex)
            {
                return new AddClaimToUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }



        //get user to claim
        public override async Task<GetUserClaimsResponse> GetUserClaims(GetUserClaimsRequest request,
         ServerCallContext context)
        {
            try
            {
                var userId = Guid.Parse(request.UserId);

                var userClaims = await userService.GetUserClaimsAsync(userId);

                var response = new GetUserClaimsResponse();

                response.Claims.AddRange(
                    userClaims.Select(claim => new ClaimDto
                    {
                        Type = claim.Type,
                        Value = claim.Value
                    })
                );

                return response;
            }
            catch (Exception ex)
            {
                return new GetUserClaimsResponse();
            }
        }




        ///add claim to role
        public override async Task<AddClaimToRoleResponse> AddClaimToRole(AddClaimToRoleRequest addClaimToRoleRequest,
            ServerCallContext context)
        {
            try
            {
                var result = await roleService.AddClaimToRoleAsync(addClaimToRoleRequest.RoleName,
                    addClaimToRoleRequest.ClaimType,
                    addClaimToRoleRequest.ClaimValue);


                return new AddClaimToRoleResponse
                {
                    Success = result,
                    Message = result ? "Claim To Role added successfully" : "Failed to add claim to role"
                };


            }
            catch (Exception ex)
            {
                return new AddClaimToRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }




        ///remove claim from user
        public override async Task<RemoveClaimFromUserResponse> RemoveClaimFromUser(RemoveClaimFromUserRequest request,
           ServerCallContext context)
        {
            try
            {
                var userId = Guid.Parse(request.UserId);
                var result = await userService.RemoveClaimFromUserAsync(userId,
                    request.ClaimType,
                    request.ClaimValue);

                return new RemoveClaimFromUserResponse
                {
                    Success = result,
                    Message = result ? "Remove Claim From User successfully" : "Failed to Remove Claim From User"
                };


            }
            catch (Exception ex)
            {
                return new RemoveClaimFromUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        //get role claims
        public override async Task<GetRoleClaimsResponse> GetRoleClaims(GetRoleClaimsRequest request,
         ServerCallContext context)
        {
            try
            {
                var claims = await roleService.GetRoleClaimsAsync(request.RoleName);

                var response = new GetRoleClaimsResponse();

                response.Claims.AddRange(
                    claims.Select(c => new ClaimDto
                    {
                        Type = c.Type,
                        Value = c.Value
                    })
                );

                return response;
            }
            catch (Exception ex)
            {
                return new GetRoleClaimsResponse();
            }
        }


        ///remove calim from role
        public override async Task<RemoveClaimFromRoleResponse> RemoveClaimFromRole(RemoveClaimFromRoleRequest request,
           ServerCallContext context)
        {
            try
            {
                var result = await roleService.RemoveClaimFromRoleAsync(
                    request.RoleName,
                    request.ClaimType,
                    request.ClaimValue);

                return new RemoveClaimFromRoleResponse
                {
                    Success = result,
                    Message = result ? "Claim removed from role" : "Failed to remove claim"
                };
            }
            catch (Exception ex)
            {
                return new RemoveClaimFromRoleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
