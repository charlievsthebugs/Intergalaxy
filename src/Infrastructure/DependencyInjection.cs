using Intergalaxy.Application.Common.Interfaces;
using Intergalaxy.Application.Interfaces;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Infrastructure.Data;
using Intergalaxy.Infrastructure.Data.Interceptors;
using Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi;
using Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Adapters;
using Intergalaxy.Infrastructure.Identity;
using Intergalaxy.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Services.Database);
        Guard.Against.Null(connectionString, message: $"Connection string '{Services.Database}' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });


        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

    
        builder.Services.AddScoped<IntergalaxyApiClient>();
        builder.Services.AddHttpClient<IntergalaxyApiClient>(
        client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ExternalApis:RickAndMorty"] ?? "");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        builder.Services.AddScoped<IIntergalaxyApiAdapter, IntergalaxyApiAdapter>();

        builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(EFWriteRepository<>));
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EFReadRepository<>));
        builder.Services.AddScoped(typeof(ICharacterRepository), typeof(EFCharacterRepository));

    }
}
