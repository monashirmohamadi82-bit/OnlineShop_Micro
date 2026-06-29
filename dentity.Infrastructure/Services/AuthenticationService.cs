using IdentityGrcpService.C2_ApplicationIdentity.Interfaces;
using IdentityGrpcService;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using Microsoft.AspNetCore.Identity;




namespace IdentityGrcpService.C3_InfrastructureIdentity.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService jwtTokenService;

        public AuthenticationService(
       UserManager<ApplicationUser> userManager,
       JwtTokenService ـjwtTokenService)
        {
            _userManager = userManager;
            jwtTokenService = ـjwtTokenService;
        }


        /// craete User in database
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = request.UserName;
                user.Email = request.Email;

                var result = await _userManager.CreateAsync(user, request.Password);


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
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var IsvalidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
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



    }
}
