using System.Windows.Input;

namespace TaskManagerUI.UI;

public partial class CustomTabBar : TabBar
{
    public static readonly BindableProperty CenterViewCommandProperty =
        BindableProperty.Create(
            nameof(CenterViewCommand),
            typeof(ICommand),
            typeof(CustomTabBar),
            default(ICommand));

    public static readonly BindableProperty CenterViewImageSourceProperty =
        BindableProperty.Create(
            nameof(CenterViewImageSource),
            typeof(ImageSource),
            typeof(CustomTabBar),
            default(ImageSource));
    public ICommand? CenterViewCommand
    {
        get => (ICommand?)GetValue(CenterViewCommandProperty);
        set => SetValue(CenterViewCommandProperty, value);
    }

    public static readonly BindableProperty CenterViewTextProperty =
    BindableProperty.Create(
        nameof(CenterViewText),
        typeof(string),
        typeof(CustomTabBar),
        default(string));

    public ImageSource? CenterViewImageSource
    {
        get => (ImageSource?)GetValue(CenterViewImageSourceProperty);
        set => SetValue(CenterViewImageSourceProperty, value);
    }

    public static readonly BindableProperty CenterViewVisibleProperty =
    BindableProperty.Create(
        nameof(CenterViewVisible),
        typeof(bool),
        typeof(CustomTabBar),
        false);

    public string? CenterViewText
    {
        get => (string?)GetValue(CenterViewTextProperty);
        set => SetValue(CenterViewTextProperty, value);
    }

    public static readonly BindableProperty CenterViewBackgroundColorProperty =
    BindableProperty.Create(
        nameof(CenterViewBackgroundColor),
        typeof(Color),
        typeof(CustomTabBar),
        default(Color));

    public bool CenterViewVisible
    {
        get => (bool)GetValue(CenterViewVisibleProperty);
        set => SetValue(CenterViewVisibleProperty, value);
    }

    public Color? CenterViewBackgroundColor
    {
        get => (Color?)GetValue(CenterViewBackgroundColorProperty);
        set => SetValue(CenterViewBackgroundColorProperty, value);
    }
}