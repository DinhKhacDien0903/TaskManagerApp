using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using TaskManagerUI.Heplpers.Extensions;
using TaskManagerUI.UI;

namespace TaskManagerUI.Handlers;

public partial class ShellHandler
{
    ShellBottomNaviHandler _shellBottomNaviHandler;

    protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
    {
        _shellBottomNaviHandler = new ShellBottomNaviHandler(this, shellItem);
        return _shellBottomNaviHandler;
    }
}

public class ShellBottomNaviHandler(IShellContext shellContext, ShellItem shellItem) : ShellBottomNavViewAppearanceTracker(shellContext, shellItem)
{
    const byte _messageTabIndex = 1;
    private readonly IShellContext shellContext = shellContext;
    BottomNavigationView _bottomNaviView;
    BadgeDrawable _messBadge;
    int? _messTabId => _bottomNaviView?.Menu?.FindItem(_messageTabIndex)?.ItemId;

    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);
        if (!Shell.GetTabBarIsVisible(shellContext.Shell.CurrentItem))
            return;

        bottomView.SetMinimumHeight(Constant.AppDimensions.MinimumTabBarHeight);
        bottomView.SetBackground(new SemiCircleBackgroundDrawable());
        if (bottomView != null)
        {
            _bottomNaviView = bottomView;
            _bottomNaviView.SetItemTextAppearanceActiveBoldEnabled(false);
        }
    }

    protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
    {
        base.SetBackgroundColor(bottomView, color);
        if (AppHelper.CurrentMainPage?.GetCurrentPage() is BasePage page && page.BackgroundColor is Color pageColor)
        {
            bottomView.RootView?.SetBackgroundColor(pageColor.ToPlatform(Colors.Transparent));
        }
    }

    public void SetMessBadge(int number)
    {
        try
        {
            if (number == 0)
            {
                _messBadge?.ClearNumber();
                if (_messTabId.HasValue)
                    _bottomNaviView?.RemoveBadge(_messTabId.Value);
                _messBadge = null;
            }
            else
            {
                var messNavi = _bottomNaviView?.Menu?.FindItem(_messageTabIndex);
                var widthMess = messNavi?.Icon?.IntrinsicWidth;
                if (_messTabId.HasValue && widthMess.HasValue && _bottomNaviView != null)
                {
                    _messBadge = _bottomNaviView.GetOrCreateBadge(_messTabId.Value);
                    _messBadge.VerticalOffset = 10;
                    _messBadge.HorizontalOffset = (int)Math.Abs((decimal)(widthMess.Value * 0.3));
                    _messBadge.BadgeTextColor = global::Android.Graphics.Color.White;
                    _messBadge.BackgroundColor = global::Android.Graphics.Color.Red;
                    _messBadge.Number = number;
                }
            }
        }
        catch (ObjectDisposedException)
        {
        }
    }

    public void ManualDispose()
    {
        _bottomNaviView?.RemoveAllViews();
        _messBadge?.Dispose();
    }
}