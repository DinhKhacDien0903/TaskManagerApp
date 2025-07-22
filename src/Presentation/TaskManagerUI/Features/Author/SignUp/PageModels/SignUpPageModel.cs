using Application.Commands;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;
using TaskManagerUI.Features.Pages;
using TaskManagerUI.Navigation;

namespace TaskManagerUI.Features.PageModels;

public partial class SignUpPageModel
    (IMediator mediator,
    INavigationService navigationService,
    INavigationOtherShellService navigationOtherShellService) : BasePageModel(navigationService)
{
    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError = false;

    private readonly IMediator _mediator = mediator;
    private readonly INavigationOtherShellService _navigationOtherShell = navigationOtherShellService;

    [RelayCommand]
    private async Task SignUpAsync()
    {
        try
        {
            // Clear previous error
            HasError = false;
            ErrorMessage = string.Empty;

            // Validate input
            if (string.IsNullOrWhiteSpace(FullName))
            {
                ShowError("Please enter your full name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowError("Please enter your email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Please enter your password.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ShowError("Please confirm your password.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                ShowError("Passwords do not match.");
                return;
            }

            // Show loading state (you can add a loading property if needed)
            var command = new SignUpCommand { Email = Email, Password = Password };
            var user = await _mediator.Send(command);

            System.Console.WriteLine($"Check log user: {user.ToJson()}");

            // Navigate to main page or verification page
            // await Shell.Current.GoToAsync("//VerificationPage");

            ShowError("Registration successful! Please check your email for verification."); // Temporary success message
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    [RelayCommand]
    async Task NavigateToSignInAsync()
    {
        if (IsBusy || NavigateToSignInCommand.IsRunning)
            return;
        try
        {
            IsBusy = true;
            await _navigationOtherShell.NavigateToAsync<SignInPage>();
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