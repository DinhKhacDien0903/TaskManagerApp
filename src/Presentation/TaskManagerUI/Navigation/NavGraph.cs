namespace TaskManagerUI.Navigation;

public static class NavGraph
{
    public static void RegisterRoute()
    {
        Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}