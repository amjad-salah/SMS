using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using SMS.Frontend.Authentication;

namespace SMS.Frontend.Extensions;

public static class ServicesContainer
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        services.AddMudServices();

        return services;
    }
}
