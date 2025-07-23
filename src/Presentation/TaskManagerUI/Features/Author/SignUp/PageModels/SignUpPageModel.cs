using System.ComponentModel.DataAnnotations;
using Application.Commands;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;
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
            HasError = false;
            ErrorMessage = string.Empty;

            var command = new SignUpCommand
            {
                Email = Email,
                Password = Password,
                FullName = FullName,
                ConfirmPassword = ConfirmPassword
            };

            var result = await _mediator.Send(command);

            System.Console.WriteLine($"Check log user: {result.ToJson()}");
        }
        catch (ValidationException ex)
        {
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            //TODO: Add logger
            System.Console.WriteLine($"Error during sign-up: {ex.Message}");
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