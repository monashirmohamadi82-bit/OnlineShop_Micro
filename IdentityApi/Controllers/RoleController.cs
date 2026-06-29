using AutoMapper;
using IdentityApi.Dto;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using IdentityGrpcService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly AuthService.AuthServiceClient _client;
        private readonly IMapper _mapper;

        public RoleController(AuthService.AuthServiceClient client, IMapper mapper)
        {

            _client = client;
            _mapper = mapper;
        }


        //create role
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(CreateRoleDto request)
        {
            var grpcRequest = _mapper.Map<CreateRoleRequest>(request);
            var result = await _client.CreateRoleAsync(grpcRequest);

            return Ok(result);
        }



        //get all role
        [HttpGet("get-role")]
        public async Task<IActionResult> GetAllRole()
        {
            var result = await _client.GetAllRolesAsync(new GetAllRolesRequest());

            return Ok(result.Roles);
        }



        //delete role
        [HttpDelete("delete-role")]
        public async Task<IActionResult> DeleteRole(DeleteRoleDto deleteRoleDto)
        {
            var grpcRequest=_mapper.Map<DeleteRoleRequest>(deleteRoleDto);
            var result=await _client.DeleteRoleAsync(grpcRequest);
            return Ok(result);

        }



        //get role calims
        [HttpGet("role-claims")]
        public async Task<IActionResult> GetRoleClaims(GetRoleClaimsDto getRoleClaimsDto)
        {

            var grpcRequest= _mapper.Map<GetRoleClaimsRequest>(getRoleClaimsDto);
            var result = await _client.GetRoleClaimsAsync(grpcRequest);

            return Ok(result.Claims);
        }



        //add claim to role 
        [HttpPost("add-claim-to-role")]
        public async Task<ActionResult> AddClaimToRole(AddClaimToRoleDto addClaimToRoleDto)
        {


            var grpcRequest = _mapper.Map<AddClaimToRoleRequest>(addClaimToRoleDto);
            var result = await _client.AddClaimToRoleAsync(grpcRequest);

            return Ok(result);

        }


        //remove claim from role
        [HttpDelete("role-claim")]
        public async Task<IActionResult> RemoveRoleClaim(RemoveRoleClaimDto dto)
        {
            var grpcRequest = _mapper.Map<RemoveClaimFromRoleRequest>(dto);

            var result = await _client.RemoveClaimFromRoleAsync(grpcRequest);

            return Ok(result);
        }

       
    }
}
