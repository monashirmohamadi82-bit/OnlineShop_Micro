using IdentityGrcpService.C2_ApplicationIdentity.Interfaces;
using IdentityGrcpService.C3_InfrastructureIdentity.Services;

namespace IdentityGrcpService.C3_InfrastructureIdentity.DependencyInjection
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<JwtTokenService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthService, AuthenticationService>();

            return services;
        }
    }
}
