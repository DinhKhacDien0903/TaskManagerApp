using Microsoft.Maui.Controls.Platform.Compatibility;
using TaskManagerUI.Navigation;
using TaskManagerUI.Services;

namespace TaskManagerUI.Handlers;
public partial class ShellItemHandler : ShellItemRenderer
{
    INavigationService _navigationService => ServiceHelper.GetService<INavigationService>();

    public EventHandler MessUnreadUpdated;

    public ShellItemHandler(IShellContext shellContext)
        : base(shellContext)
    {
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