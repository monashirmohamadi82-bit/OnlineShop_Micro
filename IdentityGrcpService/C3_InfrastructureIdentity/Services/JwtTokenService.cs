using IdentityGrpcService.C1_DomainIdentity.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityGrcpService.C3_InfrastructureIdentity.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtTokenService(IConfiguration configuration , UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            try {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
               new Claim(ClaimTypes.Email, user.Email ?? ""),
               
            };

                //get role
                var roles =await _userManager.GetRolesAsync(user);

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                //get claim
                var userClaims=await _userManager.GetClaimsAsync(user);
                foreach(var claim in userClaims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }
            

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );


                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw new Exception("Token generation failed: " + ex.Message);

            }
        
            

    }
}
}
