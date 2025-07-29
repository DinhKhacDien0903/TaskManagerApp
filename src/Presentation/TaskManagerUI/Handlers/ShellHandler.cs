using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace TaskManagerUI.Handlers;
public partial class ShellHandler : ShellRenderer, IDisposable
{
    public ShellItemHandler ShellItemHandler { get; private set; }

    public ShellHandler()
    {
    }

    protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
    {
        ShellItemHandler = new ShellItemHandler(this);
        ShellItemHandler.MessUnreadUpdated += OnMessUnreadUpdated;
        return ShellItemHandler;
    }

    private void OnMessUnreadUpdated(object sender, EventArgs e)
    {
        _shellBottomNaviHandler.SetMessBadge((int)sender);
    }

    void IDisposable.Dispose()
    {
        _shellBottomNaviHandler?.ManualDispose();
    }
}