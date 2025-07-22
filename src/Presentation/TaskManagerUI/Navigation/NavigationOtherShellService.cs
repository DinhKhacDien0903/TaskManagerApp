
namespace TaskManagerUI.Navigation
{
    public class NavigationOtherShellService : INavigationOtherShellService
    {
        private readonly Func<Type, ContentPage> _pageResolver;
        private bool _isProcessing;

        public NavigationOtherShellService(Func<Type, ContentPage> pageResolver)
        {
            _pageResolver = pageResolver;
        }

        public async Task NavigateToAsync<TPage>(object? param = null, bool isPushModal = false, bool isAnimation = true) where TPage : Page
        {
            if (_isProcessing)
                return;

            _isProcessing = true;
            try
            {
                var page = _pageResolver(typeof(TPage)) ?? throw new Exception($"Page of type {typeof(TPage).Name} not found!");
                if (page.BindingContext is not BasePageModel toPageModel)
                    throw new Exception($"PageModel for {typeof(TPage).Name} not found!");

                if (page.Parent != null && page.Parent is NavigationPage parentNav)
                    parentNav.Navigation.RemovePage(page);

                toPageModel.IsBusy = true;
                Shell.SetBackButtonBehavior(page, new BackButtonBehavior { Command = toPageModel.BackButtonCommand });
                toPageModel.IsPushPageWithNavService = true;
                await toPageModel.InitAsync(param);
                page.Appearing += OnAppearingAsync;
                page.Disappearing += OnDisappearingAsync;

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var currentPage = AppHelper.CurrentMainPage;
                    if (currentPage is NavigationPage navPage)
                    {
                        if (isPushModal)
                        {
                            await navPage.Navigation.PushModalAsync(page, isAnimation);
                        }
                        else
                        {
                            await navPage.Navigation.PushAsync(page, isAnimation);
                        }
                    }
                    else
                    {
                        var app = Microsoft.Maui.Controls.Application.Current ?? throw new InvalidOperationException("Current Application instance available.");
                        app.Windows[0].Page = new NavigationPage(page);
                    }
                });

                toPageModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex}");
                throw;
            }
            finally
            {
                _isProcessing = false;
            }
        }

        public async Task GoBackAsync()
        {
            if (_isProcessing)
                return;

            _isProcessing = true;
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var currentPage = AppHelper.CurrentMainPage;
                    if (currentPage is NavigationPage navPage && navPage.Navigation.NavigationStack.Count > 1)
                    {
                        await navPage.PopAsync();
                    }
                    else if (currentPage is NavigationPage modalNavPage && modalNavPage.Navigation.ModalStack.Count > 0)
                    {
                        await modalNavPage.Navigation.PopModalAsync();
                    }
                });
            }
            finally
            {
                _isProcessing = false;
            }
        }

        public async Task ClearNavigationStackAndNavigateToAsync<TPage>(object? param = null) where TPage : Page
        {
            if (_isProcessing)
                return;

            _isProcessing = true;

            try
            {
                var page = _pageResolver(typeof(TPage));
                if (page == null)
                    throw new Exception($"Page of type {typeof(TPage).Name} not found!");

                if (page.BindingContext is not BasePageModel toPageModel)
                    throw new Exception($"PageModel for {typeof(TPage).Name} not found!");

                Shell.SetBackButtonBehavior(page, new BackButtonBehavior { Command = toPageModel.BackButtonCommand });
                toPageModel.IsPushPageWithNavService = true;
                await toPageModel.InitAsync(param);

                page.Appearing += OnAppearingAsync;
                page.Disappearing += OnDisappearingAsync;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var app = Microsoft.Maui.Controls.Application.Current ?? throw new InvalidOperationException("Current Application instance available.");
                    app.Windows[0].Page = new NavigationPage(page);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex}");
                throw;
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private async void OnDisappearingAsync(object? sender, EventArgs? e)
        {
            if (sender is not ContentPage thisPage)
                return;

            bool isForwardNavigation = AppHelper.CurrentMainPage is NavigationPage navPage && navPage.Navigation.NavigationStack.Count > 1 && navPage.Navigation.NavigationStack[^2] == thisPage;

            if (thisPage is ContentPage)
            {
                if (!isForwardNavigation)
                {
                    var toPageModel = thisPage.BindingContext as BasePageModel;
                    if (toPageModel != null && !toPageModel.LoadDataOnAppearing)
                    {
                        thisPage.Appearing -= OnAppearingAsync;
                    }
                }

                // Gọi CallNavigatedFromAsync
                await CallNavigatedFromAsync(thisPage);
            }
        }

        private Task CallNavigatedFromAsync(ContentPage thisPage)
        {
            var fromPageModel = NaviMethodExtension.GetBasePageModel(thisPage);
            if (fromPageModel is not null)
            {
                if (AppHelper.CurrentMainPage is not NavigationPage navPage || !navPage.Navigation.NavigationStack.Contains(thisPage))
                {
                    thisPage.NavigatedFrom -= OnDisappearingAsync;
                }

                return fromPageModel.ViewIsDisAppearingAsync();
            }

            return Task.CompletedTask;
        }

        private async void OnAppearingAsync(object? sender, EventArgs? e)
        {
            if (sender is ContentPage contentPage)
            {
                var toPageModel = NaviMethodExtension.GetBasePageModel(contentPage);
                await toPageModel.ViewIsAppearingAsync();
            }
        }
    }
}