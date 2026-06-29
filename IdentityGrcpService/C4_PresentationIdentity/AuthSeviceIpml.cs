using Azure.Core;
using Google.Protobuf;
using Grpc.Core;
using IdentityGrcpService;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using IdentityGrcpService.C2_ApplicationIdentity.Interfaces;
using IdentityGrpcService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityGrcpService.C3_InfrastructureIdentity.Services;
namespace IdentityGrcpService.C4_PresentationIdentity;

public class AuthSeviceIpml : AuthService.AuthServiceBase
{

    private readonly IAuthService authService;
    private readonly IRoleService roleService;
    private readonly IUserService userService;
    public AuthSeviceIpml(IAuthService _authService,
       IRoleService _roleService, IUserService _userService)
    {
        authService = _authService;
        roleService = _roleService;
        userService = _userService;
    }


    /// craete User in database
    public override  Task<RegisterResponse> Register(RegisterRequest request,
        ServerCallContext context
        )
    {
        return authService.RegisterAsync(request);

    }




    ///Login user in database

    public override  Task<LoginResponse> Login(LoginRequest request,
        ServerCallContext context)
    {
        return authService.LoginAsync(request);
    }




    //create role 

    public override async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request,
        ServerCallContext serverCallContext)
    {
        var result = await roleService.CreateRoleAsync(request.RoleName);

        return new CreateRoleResponse
        {
            Success = result,
            Message = result ? "Create Role successfully" : "Failed to create role"
        };

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
