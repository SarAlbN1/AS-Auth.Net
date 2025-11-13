using Auth.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
