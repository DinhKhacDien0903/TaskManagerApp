using Application.Author.Commands;
using TaskManagerUI.Utilities.MVVM;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;

namespace TaskManagerUI.Features.PageModels;

public partial class SignUpPageModel(IMediator mediator) : BasePageModel
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly IMediator _mediator = mediator;

    [RelayCommand]
    private async Task SignUpAsync()
    {
        try
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = $"Passwords do not match.";
                return;
            }

            var command = new SignUpCommand { Email = Email, Password = Password };
            var user = await _mediator.Send(command);
            System.Console.WriteLine($"Check log user: {user.ToJson()}");
            ErrorMessage = $"SignUpSucess in as Verification code sent.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}