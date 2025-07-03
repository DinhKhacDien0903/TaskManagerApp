using TaskManagerUI.Helpers;
using TaskManagerUI.Utilities.MVVM;

namespace TaskManagerUI;

public partial class App : Microsoft.Maui.Controls.Application
{
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
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}