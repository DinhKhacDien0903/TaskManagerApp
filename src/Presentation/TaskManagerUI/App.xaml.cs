using TaskManagerUI.Navigation;

namespace TaskManagerUI;

public partial class App : Microsoft.Maui.Controls.Application
{
    public bool IsAppSleepingOrCovered { get; set; }

    public static BasePageModel? CurrentPageModel
    {
        get
        {
            if (Shell.Current != null && Shell.Current.CurrentPage != null)
            {
                return Shell.Current.CurrentPage?.BindingContext as BasePageModel;
            }
            else
            {
                return AppHelper.CurrentMainPage?.BindingContext as BasePageModel;
            }
        }
    }
    public App()
    {
        try
        {
            InitializeComponent();
            NavGraph.RegisterRoute();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
               {
                   if (e.ExceptionObject is Exception exception && exception != null)
                   {
                       System.Diagnostics.Debug.WriteLine($"[UnhandledException] {exception?.Message}");
                       throw exception;
                   }
               };
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during initialization
            Console.WriteLine($"Error initializing AppHelper: {ex.Message}");
            throw;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new SignInPage()));
    }
}