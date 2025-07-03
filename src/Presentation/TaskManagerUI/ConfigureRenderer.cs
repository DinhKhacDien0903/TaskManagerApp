using Android.Content.Res;
using Android.OS;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using TaskManagerUI.UI.Controls;

namespace TaskManagerUI;

public static class ConfigureRenderer
{
    public static MauiAppBuilder RegisterCompatibilityRenderer(this MauiAppBuilder builder)
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                handler.PlatformView.TextCursorDrawable?.SetTint(Android.Graphics.Color.ParseColor("#5f33e1"));
            }
#endif
        });

        return builder;
    }
}