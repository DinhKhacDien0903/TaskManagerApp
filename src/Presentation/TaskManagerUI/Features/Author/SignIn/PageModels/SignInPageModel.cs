using Application.Author.Commands;
using TaskManagerUI.Utilities.MVVM;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;
using TaskManagerUI.Helpers;
using TaskManagerUI.Features.Pages;

namespace TaskManagerUI.Features.PageModels;

public partial class SignInPageModel(IMediator mediator) : BasePageModel
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

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            // Clear previous error
            HasError = false;
            ErrorMessage = string.Empty;

            // Validate input
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

            // Show loading state (you can add a loading property if needed)
            var command = new SignInCommand { Email = Email, Password = Password };
            var user = await _mediator.Send(command);

            System.Console.WriteLine($"Check log user: {user.ToJson()}");

            // Navigate to main page or dashboard
            // await Shell.Current.GoToAsync("//MainPage");

            ShowError("Login successful!"); // Temporary success message
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private Task ForgotPasswordAsync()
    {
        // Navigate to forgot password page
        // await Shell.Current.GoToAsync("//ForgotPasswordPage");

        // For now, show a message
        ShowError("Forgot password feature coming soon!");
        return Task.CompletedTask;
    }

    [RelayCommand]
    async Task NavigateToSignUpAsync()
    {
        AppHelper.SetMainPage(new SignUpPage());
        await Task.Delay(100);
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        HasError = !string.IsNullOrWhiteSpace(message);
    }
}