using Auth.Business.Repositories;
using Auth.Business.Security;
using Auth.Data.Persistence;
using Auth.Data.Repositories;
using Auth.Data.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Auth.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuthDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'AuthDatabase' was not found.");
        }

        services.AddDbContext<AuthDbContext>(options =>
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
            options.UseMySql(
                connectionString,
                serverVersion,
                mySqlOptions =>
                {
                    mySqlOptions.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name);
                    mySqlOptions.EnableRetryOnFailure();
                });
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
