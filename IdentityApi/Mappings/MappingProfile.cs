using AutoMapper;
using IdentityApi.Dto;
using IdentityGrpcService;

namespace IdentityApi.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile( )
        {
            CreateMap<RegisterDto, RegisterRequest>();
            CreateMap<LoginDto, LoginRequest>();
            CreateMap<CreateRoleDto, CreateRoleRequest>();
            CreateMap<AssignRoleDto, AssignRoleRequest>();
            CreateMap<DeleteRoleDto, DeleteRoleRequest>();
            CreateMap<AddCliamDto,AddClaimToUserRequest>();
            CreateMap<AddClaimToRoleDto, AddClaimToRoleRequest>();
            CreateMap<RemoveClaimFromUserDto, RemoveClaimFromUserRequest>();
            CreateMap<GetRoleClaimsDto, GetRoleClaimsRequest>();
            CreateMap<GetUserClaimsDto, GetUserClaimsRequest>();
            CreateMap<RemoveRoleClaimDto, RemoveClaimFromRoleRequest>();
            CreateMap<AddClaimToUserDto, AddClaimToUserRequest>();
            CreateMap<GetUserRolesDtol, GetUserRoleRequset>();
            CreateMap<RemoveUserRoleDto, RemoveUserRoleRequset>();

        }
    }
}
