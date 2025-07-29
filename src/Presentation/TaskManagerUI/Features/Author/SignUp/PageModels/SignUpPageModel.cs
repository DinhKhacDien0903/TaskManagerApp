using Application.Commands;
using Application.Common.Extension;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;
using TaskManagerUI.Navigation;

namespace TaskManagerUI.Features.PageModels;

public partial class SignUpPageModel
    (IMediator mediator,
    INavigationOtherShellService navigationOtherShellService) : BasePageModel()
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
            ResetError();
            var command = new SignUpCommand
            {
                Email = Email,
                Password = Password,
                FullName = FullName,
                ConfirmPassword = ConfirmPassword
            };

            var result = await _mediator.Send(command);
            this.Log($"User signed up: {result.ToJson()}");
        }
        catch (ValidationException ex)
        {
            ShowError(ex.Errors.SelectMany(e => e.Value).FirstOrDefault() ?? string.Empty);
        }
        catch (Exception ex)
        {
            this.Log(ex.Message);
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

    private void ResetError()
    {
        HasError = false;
        ErrorMessage = string.Empty;
    }
}