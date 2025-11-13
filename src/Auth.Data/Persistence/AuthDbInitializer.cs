using Auth.Business.Entities;
using Auth.Business.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Auth.Data.Persistence;

public static class AuthDbInitializer
{
    public static async Task InitialiseAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("AuthDbInitializer");
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        logger.LogInformation("Pending migrations: {PendingMigrations}", string.Join(',', pendingMigrations));
        await context.Database.MigrateAsync(cancellationToken);
        await SeedAsync(context, passwordHasher, logger, cancellationToken);
    }

    private static async Task SeedAsync(AuthDbContext context, IPasswordHasher passwordHasher, ILogger logger, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        logger.LogInformation("Seeding default administrator user.");

        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = passwordHasher.Hash("Admin123$"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(adminUser, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Administrator user created with username 'admin'. Remember to change the password immediately.");
    }
}
