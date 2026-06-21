using AutoMapper;
using IdentityApi.Dto;
using IdentityGrpcService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AuthService.AuthServiceClient _client;
        private readonly IMapper _mapper;

        public UserController(AuthService.AuthServiceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }


        //assing user to role
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDto request)
        {
            var grpcRequest = _mapper.Map<AssignRoleRequest>(request);
            var result = await _client.AssignRoleToUserAsync(grpcRequest);

            return Ok(result);
        }

        //get user roles
        [Authorize(Roles = "Admin")]
        [HttpGet("get-user-role")]
        public async Task<IActionResult> GetUserRoles(GetUserRolesDtol request)
        {
            var grpcRequest = _mapper.Map<GetUserRoleRequset>(request);
            var result = await _client.GetUserRoleAsync(grpcRequest);

            return Ok(result);
        }

        //remove user role
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user-role")]
        public async Task<IActionResult> RemoveUserRole(RemoveUserRoleDto request)
        {
            var grpcRequest = _mapper.Map<RemoveUserRoleRequset>(request);
            var result = await _client.RemoveUserRoleAsync(grpcRequest);

            return Ok(result);
        }


        //add claim to user
        [Authorize(Roles = "Admin")]
        [HttpPost("add-claim-to-user")]
        public async Task<IActionResult> AddClaimToUser(AddClaimToUserRequest addCliamDto)
        {

            var grpcRequest = _mapper.Map<AddClaimToUserRequest>(addCliamDto);
            var result = await _client.AddClaimToUserAsync(grpcRequest);
            return Ok(result);
        }



        //get claim to user
        [HttpGet("get-claims-user")]
        public async Task<IActionResult> GetUserClaims(GetUserClaimsDto getUserClaimsDto)
        {
            var grpcRequest = _mapper.Map<GetUserClaimsRequest>(getUserClaimsDto); 

            var result = await _client.GetUserClaimsAsync(grpcRequest);

            return Ok(result.Claims);
        }


        //remove claim  from user
        [Authorize(Roles = "Admin")]
        [HttpDelete("remove-claim-from-user")]
        public async Task<IActionResult> RemoveClaimFromUser(RemoveClaimFromUserDto dto)
        {
                   var grpcRequest =_mapper.Map<RemoveClaimFromUserRequest>(dto);

            var result =await _client.RemoveClaimFromUserAsync(grpcRequest);

            return Ok(result);
        }


    }
}
