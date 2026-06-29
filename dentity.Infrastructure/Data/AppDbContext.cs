using IdentityGrcpService.C3_InfrastructureIdentity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using Microsoft.EntityFrameworkCore;


public class AppDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}