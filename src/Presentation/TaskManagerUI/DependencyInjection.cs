using TaskManagerUI;
using TaskManagerUI.Features.PageModels;
using TaskManagerUI.Features.Pages;
using TaskManagerUI.Navigation;

namespace AuthorApp;

public static class DependencyInjection
{
    public static IServiceCollection AddApplications(this IServiceCollection services)
    {
#if ANDROID
        services.AddSingleton<INavigationService, NavigationService>();
#endif
        return services;
    }

    public static IServiceCollection RegisterPageModels(this IServiceCollection services)
    {
        services.AddTransient<SignInPageModel>();
        services.AddTransient<SignUpPageModel>();
        return services;
    }

    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services.AddTransient<MainPage>();
        services.AddTransient<SignInPage>();
        services.AddTransient<SignUpPage>();

        return services;
    }
}