using Android.Views;
using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using TaskManagerUI.Heplpers.Extensions;
using AWindow = Android.Views.Window;

namespace TaskManagerUI.Services
{
    public class SystemStyleManager : ISystemStyleManager
    {
        public void SetBackGroundDrawable(string? hexColor)
        {
            var currentWindow = GetCurrentWindow();
            if (currentWindow == null)
                return;

            var currentMainPage = AppHelper.CurrentMainPage;
            var currentPageBackgroundColor = currentMainPage?.GetCurrentPage()?.BackgroundColor;

            //TODO: if currentPageBackgroundColor is not null and not transparent, use it instead of hexColor

            currentWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(
                string.IsNullOrEmpty(hexColor) ? global::Android.Graphics.Color.Transparent : global::Android.Graphics.Color.ParseColor(hexColor)));
        }

        public void SetNavigationBarColor(string hexColor)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentWindow = GetCurrentWindow();
                if (OperatingSystem.IsAndroidVersionAtLeast(35))
                {
                    if (currentWindow?.DecorView is not Android.Views.View decorView)
                        return;

                    var color = Android.Graphics.Color.ParseColor(hexColor);
                    decorView.SetBackgroundColor(color);
                    AppHelper.CurrentMainPage?.GetCurrentPage()?.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(Color.FromArgb(hexColor));
                }
                else
                {
                    currentWindow?.SetNavigationBarColor(global::Android.Graphics.Color.ParseColor(hexColor));
                }
            });
        }

        public void SetStatusBarColor(string hexColor, bool isAnimated = false)
        {
            var currentWindow = GetCurrentWindow();
            if (currentWindow == null)
                return;

            try
            {
                var color = global::Android.Graphics.Color.ParseColor(hexColor);
                if (OperatingSystem.IsAndroidVersionAtLeast(35))
                {
                    var activity = Platform.CurrentActivity;
                    var currentMainPage = AppHelper.CurrentMainPage;
                    if (currentMainPage != null && currentMainPage.Navigation.ModalStack.Count > 0)
                    {
                        var fragmentManager = (activity as MauiAppCompatActivity)?.SupportFragmentManager;
                        var targetFragment = fragmentManager?.Fragments
                            .OfType<AndroidX.Fragment.App.DialogFragment>()
                            .FirstOrDefault();
                        if (targetFragment != null)
                        {
                            currentWindow = targetFragment?.Dialog.Window;
                            // TODO: waiting for https://github.com/CommunityToolkit/Maui/issues/2370
#pragma warning disable CA1422 // Validate platform compatibility
                            currentWindow?.SetStatusBarColor(color);
                            currentWindow?.SetNavigationBarColor(color);
#pragma warning restore CA1422 // Validate platform compatibility
                        }

                        return;
                    }

                    var rootView = currentWindow.DecorView.RootView;
                    if (currentWindow.DecorView is ViewGroup decorView)
                    {
                        var oldStatusBar = decorView.FindViewWithTag("status_bar");
                        if (oldStatusBar != null)
                        {
                            decorView.RemoveView(oldStatusBar);
                        }

                        var statusBarInsets = rootView?.OnApplyWindowInsets(currentWindow?.DecorView?.RootWindowInsets)
                            .GetInsets(WindowInsets.Type.StatusBars());
                        var statusBarView = new Android.Widget.GridLayout(rootView?.Context)
                        {
                            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, statusBarInsets?.Top ?? 0),
                            Background = new Android.Graphics.Drawables.ColorDrawable(color),
                            Tag = "status_bar"
                        };
                        decorView.AddView(statusBarView, 0);
                    }

                    rootView?.RequestApplyInsets();
                }
                else
                {
                    currentWindow.SetStatusBarColor(color);
                }
            }
            catch (Exception)
            {
            }
        }

        AWindow? GetCurrentWindow()
        {
            var currentMainPage = AppHelper.CurrentMainPage;
            if (currentMainPage != null && currentMainPage.Navigation.ModalStack.Count > 0)
            {
                var fragmentManager = (Platform.CurrentActivity as MauiAppCompatActivity)?.SupportFragmentManager;
                var targetFragment = fragmentManager?.Fragments
                    .OfType<AndroidX.Fragment.App.DialogFragment>()
                    .FirstOrDefault();
                return targetFragment?.Dialog?.Window;
            }

            var window = Platform.CurrentActivity?.Window;
            if (window == null)
                return null;
            try
            {
                // clear FLAG_TRANSLUCENT_STATUS flag:
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

                return window;
            }
            catch
            {
                return null;
            }
        }
    }
}