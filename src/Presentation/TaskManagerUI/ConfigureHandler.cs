using System;
using TaskManagerUI.Handlers;

namespace TaskManagerUI;

public static class ConfigureHandler
{
    public static MauiAppBuilder RegisterHandlers(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers((handlers) =>
        {
            #region General Hanlers
            // AllowMultiLineTruncation();
            // handlers.AddHandler(typeof(Picker), typeof(CustomPickerHandler));
            handlers.AddHandler(typeof(Shell), typeof(ShellHandler));
            // handlers.AddHandler(typeof(BorderlessEntry), typeof(BorderlessEntryHandler));
            // handlers.AddHandler(typeof(Button), typeof(CustomButtonHandler));
            // handlers.AddHandler(typeof(CustomCommunityPopup), typeof(CustomCommunityPopupHandler));
            // handlers.AddHandler(typeof(Editor), typeof(CustomEditorHandler));
            // handlers.AddHandler(typeof(SearchBar), typeof(CustomSearchBarHandler));
            // handlers.AddHandler(typeof(Entry), typeof(CustomEntryHandler));
            // handlers.AddHandler(typeof(ScrollView), typeof(CustomScrollViewHandler));
            #endregion
            // Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("CustomizationControls", (handler, view) =>
            // {
            //     if (view is CustomTitleLabel)
            //         AndroidX.Core.Widget.TextViewCompat.SetTextAppearance(handler.PlatformView, Resource.Style.TextAppearance_AppCompat_Title_Inverse);
            // });

            // WebViewHandler.Mapper.ModifyMapping("WebViewClient", (handler, view, previous) =>
            // {
            //     if (handler is CustomWebViewHandler mauiHandler)
            //         handler.PlatformView.SetWebViewClient(new CustomWebViewHandler.CustomWebViewClient(mauiHandler));
            // });

            // handlers.AddHandler(typeof(Page), typeof(NotifyPageHandler));
            // handlers.AddHandler(typeof(RefreshView), typeof(CustomRefreshViewHandler));
            // handlers.AddHandler(typeof(CustomWebView), typeof(CustomWebViewHandler));
            // handlers.AddHandler(typeof(WebView), typeof(DefaultWebViewHandler));
            // handlers.AddHandler(typeof(Label), typeof(CustomLabelHandler));

        });
        return builder;
    }

    //     public static void AllowMultiLineTruncation()
    //     {
    //         static void UpdateMaxLines(Microsoft.Maui.Handlers.LabelHandler handler, ILabel label)
    //         {
    //             var textView = handler.PlatformView;
    // #if ANDROID
    //             if (label is Label controlsLabel
    //                 && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End && controlsLabel.MaxLines != -1)
    //             {
    //                 textView.SetMaxLines(controlsLabel.MaxLines);
    //             }
    // #elif IOS
    //                 if (label is Label controlsLabel
    //                           && textView.LineBreakMode == UILineBreakMode.TailTruncation)
    //                 {
    //                     textView.Lines = controlsLabel.MaxLines;
    //                 }
    // #endif
    //         }

    //         LabelHandler.Mapper.AppendToMapping(
    //            nameof(Label.LineBreakMode), (h, v) => UpdateMaxLines((LabelHandler)h, v));

    //         LabelHandler.Mapper.AppendToMapping(
    //           nameof(Label.MaxLines), (h, v) => UpdateMaxLines((LabelHandler)h, v));
    //     }
}