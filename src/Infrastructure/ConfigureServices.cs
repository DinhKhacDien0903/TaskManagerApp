using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.Services;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyB6K0UKaxfE0lM2KZDgo18vXdDezQmFHE4",
            AuthDomain = "mauiauthorwithfirebase.firebaseapp.com",
            Providers = [
                new EmailProvider()
            ]
        }));

        services.AddScoped<IAuthorService, FirebaseAuthorService>();

        return services;
    }
}