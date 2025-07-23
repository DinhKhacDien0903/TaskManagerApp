using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;
using TaskManagerUI.Navigation;
using Application.Commands;

namespace TaskManagerUI.Features.PageModels;

public partial class SignInPageModel
    (IMediator mediator,
    INavigationService navigationService,
    INavigationOtherShellService navigationOtherShellService) : BasePageModel(navigationService)
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError = false;

    private readonly IMediator _mediator = mediator;
    private readonly INavigationOtherShellService _navigationOtherShellService = navigationOtherShellService;

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            HasError = false;
            ErrorMessage = string.Empty;

            var command = new SignInCommand { Email = Email, Password = Password };
            var user = await _mediator.Send(command);

            System.Console.WriteLine($"Check log user: {user.ToJson()}");
        }
        catch (ValidationException ex)
        {
            ShowError(ex.Errors.SelectMany(e => e.Value).FirstOrDefault() ?? string.Empty);
        }
        catch (Exception ex)
        {
            //TODO: Add logger
            System.Console.WriteLine($"Error during sign-in: {ex.Message}");
        }
    }

    [RelayCommand]
    private Task ForgotPasswordAsync()
    {
        ShowError("Forgot password feature coming soon!");
        return Task.CompletedTask;
    }

    [RelayCommand]
    async Task NavigateToSignUpAsync()
    {
        if (IsBusy || NavigateToSignUpCommand.IsRunning)
            return;
        try
        {
            IsBusy = true;
            await _navigationOtherShellService.NavigateToAsync<SignUpPage>();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = !string.IsNullOrWhiteSpace(message);
    }
}