using CommunityToolkit.Maui.Alerts;
using System.Windows.Input;
using TaskManagerUI.Handlers;
using TaskManagerUI.Heplpers.Extensions;
using TaskManagerUI.Services;

namespace TaskManagerUI;

public partial class AppShell : Shell
{
    public static bool Initialized { get; set; }
    public int TotalUnreadMessage { get; set; }
    public string LastestMessageTitle { get; private set; }
    public bool IsNavigating { get; set; }
    //private readonly IDataService _dataService;
    private ShellNavigationSource _currentShellNavigationSource;
    public IEnumerable<Page> PreviousPageStack { get; set; }
    public bool IsTearingDown { get; private set; }

    // To fix bug status bar color when tap to bototm bar icon then the home location change!
    string _homeLocation = "_";

    public AppShell()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (IsTearingDown)
            return;
        CallUpdateMaster();
        //if (_dataService.UpdateMasterTask != null && !_dataService.UpdateMasterTask.Task.IsCompleted)
        //    await _dataService.UpdateMasterTask.Task;
        //MainThread.BeginInvokeOnMainThread(async () =>
        //{
        //    await _dataService.GetMessageAsync();
        //});
    }

    public static void CallUpdateMaster()
    {
        //var onForeground = ServiceHelper.GetService<IDeviceService>().OnForeground;
        //if (!Initialized && onForeground)
        //{
        //    WeakReferenceMessenger.Default.Send(new UpdateMasterMessage(true));
        //    ErrorHelper.ShowMaintenanceDialog();
        //    Initialized = true;
        //}
    }

    protected async override void OnNavigating(ShellNavigatingEventArgs args)
    {
        if (IsTearingDown)
            return;
        _currentShellNavigationSource = args.Source;
        if (args.Source != ShellNavigationSource.Unknown)
        {
            IsNavigating = true;
        }

        await Task.Delay(1);
        //        var targetOrignalString = args.Target?.GetFullLocation()?.OriginalString;
        //        var currentOrignalString = args.Current?.GetFullLocation()?.OriginalString;
        //        if (targetOrignalString != null && currentOrignalString != null
        //           && currentOrignalString == targetOrignalString
        //           && targetOrignalString.Contains("/home"))
        //        {
        //            if (_homeLocation == targetOrignalString)
        //            {
        //                IsNavigating = false;
        //                return;
        //            }

        //            _homeLocation = targetOrignalString;
        //        }

        //#if ANDROID
        //        var isAutoStart = UserSetting.Get(StorageKey.IsAutoStart);
        //        if (isAutoStart == null || !Convert.ToBoolean(isAutoStart))
        //        {
        //            UserSetting.Set(StorageKey.IsAutoStart, true.ToString());
        //            await ServiceHelper.GetService<IOpenSetting>().OpenAutoStartSettingScreenAsync();
        //        }
        //#endif
    }

    protected async override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if (IsTearingDown)
            return;
        //var currentSectionStack = GetCurrentSectionStack();
        //if (_currentShellNavigationSource != ShellNavigationSource.ShellSectionChanged
        //    && PreviousPageStack is not null
        //    && PreviousPageStack.Count() > currentSectionStack.Count)
        //{
        //    foreach (var page in PreviousPageStack.Except(currentSectionStack))
        //    {
        //        var vm = page?.BindingContext as BaseViewModel;
        //        if (page is null)
        //            continue;
        //        SetBackButtonBehavior(page, null);
        //        await vm.ViewIsRemovedAsync();
        //    }

        //    if (_currentShellNavigationSource == ShellNavigationSource.Pop && PreviousPageStack.Last() is ICameraMauiPage)
        //    {
        //        IndicateBusyDuringCameraRelease(currentSectionStack.Last().BindingContext as BaseViewModel);
        //    }
        //}

        //PreviousPageStack = currentSectionStack;
        //var currentOrignalString = args.Current?.GetFullLocation()?.OriginalString;
        //if (currentOrignalString.EndsWith("/home", System.StringComparison.Ordinal)
        //    || currentOrignalString == _homeLocation || currentOrignalString.Contains("SelectCountryPage", System.StringComparison.Ordinal))
        //{
        //    ServiceHelper.GetService<ISystemStyleManager>().SetStatusBarColor(ThemeUtil.GetResourceColorByKey("BackgroundCoverColor").GetHexString(), false);
        //}
        //else
        //{
        //    var navigation = Shell.Current.CurrentPage.Navigation;
        //    if (navigation.ModalStack.LastOrDefault() is QRCodeScanPage || navigation.ModalStack.LastOrDefault() is BarCodeScanPage)
        //    {
        //        await CheckPermision.CheckPermisionAsync(Permission.Camera);
        //    }

        ServiceHelper.GetService<ISystemStyleManager>().SetStatusBarColor(Constant.AppStyle.PrimaryColor);
        //}

        // Called when app resume
        CallUpdateMaster();
        IsNavigating = false;
    }

    protected override bool OnBackButtonPressed()
    {
        if (IsNavigating)
            return true;
        var currentPage = CurrentItem?.CurrentItem?.Stack?.LastOrDefault();
        if (currentPage != null)
            return HandleBackButtonForPage(currentPage);
        return HandleBackButtonForTab();
    }

    private bool HandleBackButtonForTab()
    {
        //if (this.CurrentPage is HomePage)
        //{
        //    (Platform.CurrentActivity as MainActivity)?.CustomMoveTaskToBack();
        //}
        //else
        //{
        //    MainThread.BeginInvokeOnMainThread(async () =>
        //    {
        //        await CoreMethodsExtensions.SwitchTabAsync<HomePage>();
        //    });
        //}

        return true;
    }

    private bool HandleBackButtonForPage(Page currentPage)
    {
        var currentVM = App.CurrentPageModel;
        if (currentVM != null && currentVM.IsBusy)
            return true;
        return false;
    }

    public void SetTotalUnReadMessage(int totalUnread, string titleMessage = "")
    {
        if (totalUnread != TotalUnreadMessage)
        {
            TotalUnreadMessage = totalUnread;
            if (this.Handler is ShellHandler shellHandler)
            {
                Dispatcher.Dispatch(() => shellHandler?.ShellItemHandler?.MessUnreadUpdated?.Invoke(TotalUnreadMessage, null));
            }
        }

        LastestMessageTitle = titleMessage;
        //WeakReferenceMessenger.Default.Send(new SetContentBubbleMessage(titleMessage));
        //ServiceHelper.GetService<IDeviceService>().SetBadgeIcon(totalUnread == 0 ? 0 : 1);
    }

    private List<Page> GetCurrentSectionStack()
    {
        var result = new List<Page>();
        foreach (var item in this.CurrentItem.CurrentItem.Navigation.NavigationStack)
        {
            if (item is null)
                continue;
            result.Add(item);
        }

        foreach (var item in this.CurrentItem.CurrentItem.Navigation.ModalStack)
        {
            if (item is null)
                continue;
            result.Add(item);
        }

        return result;
    }

    public async Task RemoveRootAsync()
    {
        FFImageLoading.IImageService imageService = FFImageLoading.ImageService.Instance;
        //await imageService.LoadCompiledResource("snapshot_image.png").WithCache(FFImageLoading.Cache.CacheType.All).PreloadAsync(imageService);
        this.RefreshCurrentTab();
        IsTearingDown = true;
        //ServiceHelper.GetService<IScreenOverlayService>().ShowOverlay();
        Shell.SetTabBarIsVisible(this, false);
        Shell.SetNavBarIsVisible(this, false);

        List<Task> vmRemovalTasks = new();
        List<Page> pages = new();
        List<Task> popTasks = new();
        foreach (var item in this.CurrentItem.Items)
        {
            if (item.Navigation.NavigationStack.Count > 1)
            {
                if (OperatingSystem.IsAndroid())
                {
                    popTasks.Add(item.Navigation.PopToRootAsync(false));
                }
                else
                {
                    this.CurrentItem.CurrentItem = item;
                    await item.Navigation.PopToRootAsync(false);
                    popTasks.Add(Task.Delay(50));
                }
            }

            if ((item.CurrentItem as IShellContentController)?.Page is not { } page)
                continue;
            pages.Add(page);
            if (page.BindingContext is not BasePageModel viewModel)
                continue;
            var task = viewModel?.ViewIsRemovedAsync();
            if (task != null)
                vmRemovalTasks.Add(task);
        }

        await Task.WhenAll(popTasks);
        await Task.Delay(100);
        await RemoveTabBarElementManuallyAsync();
        await Task.WhenAll(vmRemovalTasks);
        foreach (var page in pages)
        {
            page.TearDown();
        }

        IsTearingDown = false;
    }

    private async Task RemoveTabBarElementManuallyAsync()
    {
        if (this.Items is null)
            return;
        var tabBar = this.Items[0] as TabBar;
        tabBar?.Items?.Clear();
        this.Items.Clear();
        this.Items.Add(new ShellContent()
        {
            Content = new ContentPage()
        });
        await Task.Yield();
    }

    public ICommand CenterViewCommand { get; } = new Command(async () => await Toast.Make("CenterViewCommand invoked!").Show());
}