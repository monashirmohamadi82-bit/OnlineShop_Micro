

using IdentityGrcpService.C2_ApplicationIdentity.Interfaces;
using IdentityGrcpService.C3_InfrastructureIdentity.DependencyInjection;
using IdentityGrcpService.C3_InfrastructureIdentity.Services;
using IdentityGrcpService.C4_PresentationIdentity;
using IdentityGrpcService.C1_DomainIdentity.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddInfrastructure();

var app = builder.Build();


app.MapGrpcService<AuthSeviceIpml>();





app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
