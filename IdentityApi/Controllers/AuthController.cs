using AutoMapper;
using IdentityApi.Dto;
using IdentityGrpcService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService.AuthServiceClient _client;
        private readonly IMapper _mapper;

        public AuthController(AuthService.AuthServiceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }


        //register user 
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {

            var grpcRequest = _mapper.Map<RegisterRequest>(request);
            var result = await _client.RegisterAsync(grpcRequest);

            return Ok(result);
        }



        ///login  User
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {

            var grpcRequest= _mapper.Map<LoginRequest>(request);
            var result = await _client.LoginAsync(grpcRequest);

            return Ok(result);
        }









    }
}