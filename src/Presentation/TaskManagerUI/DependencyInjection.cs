using TaskManagerUI;
using TaskManagerUI.Navigation;
using TaskManagerUI.Services;

namespace AuthorApp;

public static class DependencyInjection
{
    public static IServiceCollection AddApplications(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISystemStyleManager, SystemStyleManager>();
        return services;
    }

    public static IServiceCollection RegisterPageModels(this IServiceCollection services)
    {
        services.AddTransient<SignInPageModel>();
        services.AddTransient<SignUpPageModel>();
        services.AddTransient<WellcomePageModel>();
        services.AddTransient<HomePage>();
        services.AddTransient<CalenderPage>();
        services.AddTransient<TaskPage>();
        services.AddTransient<NotePage>();
        services.AddTransient<ProfilePage>();
        return services;
    }

    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services.AddTransient<MainPage>();
        services.AddSingleton<SignInPage>();
        services.AddSingleton<SignUpPage>();
        services.AddSingleton<WellcomePage>();
        services.AddSingleton<HomePageModel>();
        services.AddSingleton<CalenderPageModel>();
        services.AddSingleton<TaskPageModel>();
        services.AddSingleton<NotePageModel>();
        services.AddSingleton<ProfilePageModel>();

        return services;
    }
}