using CommunityToolkit.Mvvm.Input;
using TaskManagerUI.Helpers;
using TaskManagerUI.IServices;
using TaskManagerUI.Navigation;

namespace TaskManagerUI.Utilities.MVVM;

public abstract partial class BasePageModel : ObservableObject
{
    #region Properties
    public bool IsPushPageWithNavService { get; set; }
    private object? _initData;
    public bool Initialized;
    public readonly bool LoadDataOnAppearing;
    public bool IsNavigationInProgress
    {
        get
        {
            return IsBusy ||
                    BackButtonCommand.IsRunning ||
                    NavigationService != null && NavigationService is NavigationService { IsProcessing: true };
        }
    }
    public INavigationService NavigationService { get; private set; }
    protected virtual bool ShouldLoadData { get; set; } = true;

    [ObservableProperty]
    private bool _isBusy;
    [ObservableProperty]
    private bool _isRefreshing;
    #endregion

    #region Commands
    [RelayCommand]
    private async Task OnBackButtonAsync(Action action = null)
    {
        if (IsNavigationInProgress)
            return;
        IsBusy = true;
        if (action is not null)
            action.Invoke();

        await NavigationService.PopPageAsync();
        await Task.Delay(100).ContinueWith(t =>
        {
            IsBusy = false;
        }).ConfigureAwait(false);
    }
    #endregion

    #region Methods

    protected BasePageModel(bool loadDataOnAppearing = true)
    {
        NavigationService = NavigationService = ServiceHelper.GetService<INavigationService>();
        LoadDataOnAppearing = loadDataOnAppearing;
    }

    public virtual Task InitializeAsync()
    {
        byte count = 1;
        while (Shell.Current?.CurrentPage?.Parent == null && count < 21)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(count * 100);
            });

            count++;
        }

        Initialized = true;
        return Task.CompletedTask;
    }
    public virtual Task OnAppearingAsync()
    {
        System.Diagnostics.Debug.WriteLine($"{GetType().Name}.{nameof(OnAppearingAsync)}");

        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync()
    {
        System.Diagnostics.Debug.WriteLine($"{GetType().Name}.{nameof(OnDisappearingAsync)}");

        return Task.CompletedTask;
    }

    public virtual Task LoadDataAsync()
    {
        return Task.FromResult(true);
    }

    public virtual Task InitAsync(object? initData)
    {
        try
        {
            _initData = initData;
            if (App.CurrentPageModel != null && this == App.CurrentPageModel)
            {
                ServiceHelper.GetService<IOrientationService>().Portrait();
            }
        }
        catch (Exception)
        {
            throw;
        }
        return Task.CompletedTask;
    }

    public async virtual Task ViewIsAppearingAsync()
    {
        IsBusy = true;
        var initialized = Initialized;
        if (!initialized)
            await InitializeAsync();
        if (ShouldLoadData && (LoadDataOnAppearing || !initialized))
        {
            await MainThread.InvokeOnMainThreadAsync(LoadDataAsync);
        }

        IsBusy = false;
    }

    private bool _isDisappeared;
    public virtual Task ViewIsDisAppearingAsync()
    {
        var currentActivity = Platform.CurrentActivity;
        var currentFocus = currentActivity?.Window?.CurrentFocus;

        _isDisappeared = true;

        return Task.CompletedTask;
    }

    public virtual Task ViewIsRemovedAsync()
    {
        if (!_isDisappeared)
        {
            ViewIsDisAppearingAsync();
        }

        return Task.CompletedTask;
    }

    #endregion
}