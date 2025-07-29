using System.Diagnostics;
using TaskManagerUI.Heplpers.Extensions;
namespace TaskManagerUI.Helpers;

public static class AppHelper
{
    private const string HexColorStatusBarStart = "#ffffff";

    public static Page? CurrentMainPage
    {
        get
        {
            try
            {
                var currentPage = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault(w => w.Page != null)?.Page;
                currentPage ??= Microsoft.Maui.Controls.Application.Current?.Windows[0].Page;
                return currentPage ?? throw new InvalidOperationException("Current page is null, ensure the app has valid page!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(("Error when get current page " + ex.Message));
                return null;
            }
        }
    }

    public static void SetMainPage(Page newPage)
    {
        try
        {
            if (newPage == null)
                throw new ArgumentNullException(nameof(newPage), "New MainPage cannot be null.");

            var app = Microsoft.Maui.Controls.Application.Current ?? throw new InvalidOperationException("Current Application instance available.");
            if (app.Windows.Count == 0)
                throw new InvalidOperationException("No Windows available in the current Application.");
            // #if ANDROID
            //             if (OperatingSystem.IsAndroidVersionAtLeast(35) && newPage is not AppShell)
            //             {
            //                 var bottomPadding = ServiceHelper.GetService<IDeviceService>().GetNavigationBarHeight();
            //                 var topPadding = ServiceHelper.GetService<IDeviceService>().GetStatusBarHeight();
            //                 newPage.Padding = new Thickness(0, topPadding, 0, bottomPadding);
            //             }
            // #endif

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var check = newPage is AppShell;
                app.Windows[0].Page = newPage is AppShell ? newPage : new NavigationPage(newPage);
            });
        }
        catch
        {
            throw;
        }
    }

    internal static async Task RefreshAppAsync()
    {
        if (Microsoft.Maui.Controls.Application.Current is App { IsAppSleepingOrCovered: true })
            return;
        var oldShell = Shell.Current;
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (oldShell is AppShell appShell)
            {
                await appShell.RemoveRootAsync();
                await CoreMethodsExtensions.ForceGarbageCollectorAsync();
            }

            var newShell = new AppShell();
            if (oldShell != null)
            {
                // TODO: Workaround - Shell Page Memory Leak https://github.com/dotnet/maui/issues/22645  
                newShell.Loaded += OnNewShellLoaded;
            }

            SetMainPage(newShell); // REQUIRE RUN MAIN THREAD  

            void OnNewShellLoaded(object? sender, EventArgs e)
            {
                oldShell?.TearDown();
                oldShell = null;
                //ServiceHelper.GetService<ISystemStyleManager>().SetStatusBarColor(ThemeUtil.GetBackgroundCoverColor());  
                //ServiceHelper.GetService<IScreenOverlayService>().RemoveOverlay();  
                newShell.Loaded -= OnNewShellLoaded;
            }
        });
    }
}