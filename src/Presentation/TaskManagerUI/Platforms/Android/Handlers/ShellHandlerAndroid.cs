using Android.Graphics.Drawables;
using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using static Android.Views.ViewGroup;

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

public class ShellBottomNaviHandler : ShellBottomNavViewAppearanceTracker
{
    const byte _messageTabIndex = 1;

    BottomNavigationView _bottomNaviView;
    BadgeDrawable _messBadge;
    int? _messTabId => _bottomNaviView?.Menu?.FindItem(_messageTabIndex)?.ItemId;

    public ShellBottomNaviHandler(IShellContext shellContext, ShellItem shellItem)
        : base(shellContext, shellItem)
    {
    }

    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);
        var tabbarDrawable = new GradientDrawable();
        //tabbarDrawable.SetCornerRadii([
        //    80f, 80f,
        //    80f, 80f,
        //    0f, 0f,
        //    0f, 0f
        // ]);

        tabbarDrawable.SetCornerRadius(50);

        tabbarDrawable.SetColor(appearance.EffectiveTabBarBackgroundColor.ToPlatform());
        bottomView.SetBackground(tabbarDrawable);
        if (bottomView != null)
        {
            _bottomNaviView = bottomView;
            _bottomNaviView.SetItemTextAppearanceActiveBoldEnabled(false);
            if (bottomView.LayoutParameters is MarginLayoutParams marginLayout)
            {
                marginLayout.SetMargins(50, 0, 50, 50);
                bottomView.LayoutParameters = marginLayout;
            }
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