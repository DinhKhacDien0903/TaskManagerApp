using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using TaskManagerUI.Navigation;
using TaskManagerUI.Services;
using TaskManagerUI.UI;
using View = Android.Views.View;

namespace TaskManagerUI.Handlers;

public partial class ShellItemHandler(IShellContext shellContext) : ShellItemRenderer(shellContext)
{
    INavigationService _navigationService => ServiceHelper.GetService<INavigationService>();

    public EventHandler MessUnreadUpdated;

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);
        if (Context is not null && ShellItem is CustomTabBar { CenterViewVisible: true } tabBar)
        {
            var rootLayout = new FrameLayout(Context)
            {
                LayoutParameters =
                    new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };

            const int middleViewSize = 150;
            rootLayout.AddView(view);

            var middleViewLayoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                                      ViewGroup.LayoutParams.WrapContent,
                                                                      GravityFlags.CenterHorizontal |
                                                                      GravityFlags.Bottom)
            {
                BottomMargin = 100,
                Width = middleViewSize,
                Height = middleViewSize
            };
            var middleView = new Android.Widget.Button(Context)
            {
                LayoutParameters = middleViewLayoutParams
            };
            middleView.Click += delegate
            {
                tabBar.CenterViewCommand?.Execute(null);
            };
            middleView.SetPadding(20, 20, 20, 20);
            if (tabBar.CenterViewBackgroundColor is not null)
            {
                var backgroundView = new View(Context)
                {
                    LayoutParameters = middleViewLayoutParams
                };
                var backgroundDrawable = new GradientDrawable();
                backgroundDrawable.SetShape(ShapeType.Rectangle);
                backgroundDrawable.SetCornerRadius(middleViewSize / 2f);
                backgroundDrawable.SetColor(tabBar.CenterViewBackgroundColor.ToPlatform(Colors.Transparent));
                backgroundView.SetBackground(backgroundDrawable);
                rootLayout.AddView(backgroundView);
            }

            var context = tabBar.Window?.Page?.Handler?.MauiContext ?? Microsoft.Maui.Controls.Application.Current?.Windows.LastOrDefault()?.Page?.Handler?.MauiContext;
            tabBar.CenterViewImageSource?.LoadImage(context!, result =>
            {
                if (result?.Value is not BitmapDrawable drawable || drawable.Bitmap is null)
                {
                    return;
                }

                const int padding = 20;
                middleView.LayoutParameters = new FrameLayout.LayoutParams(
                    drawable.Bitmap.Width - padding, drawable.Bitmap.Height - padding,
                    GravityFlags.CenterHorizontal | GravityFlags.Bottom)
                {
                    BottomMargin = middleViewLayoutParams.BottomMargin + (int)(1.5 * padding)
                };
                middleView.SetBackground(drawable);
                middleView.SetMinimumHeight(0);
                middleView.SetMinimumWidth(0);
            });

            rootLayout.AddView(middleView);
            return rootLayout;
        }

        return view;
    }

    void PerformTabReselected()
    {
        var currentVM = App.CurrentPageModel;
        if (currentVM != null && _navigationService != null)
        {
            if (_navigationService?.GetNumberStack() == 1 && currentVM is IScrollTopOnReselect)
            {
                ((IScrollTopOnReselect)currentVM).ScrollToTop();
            }
        }
    }
}