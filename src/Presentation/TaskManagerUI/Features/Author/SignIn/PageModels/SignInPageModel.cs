using Application.Author.Commands;
using TaskManagerUI.Utilities.MVVM;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MongoDB.Bson;

namespace TaskManagerUI.Features.PageModels;

public partial class SignInPageModel(IMediator mediator) : BasePageModel
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly IMediator _mediator = mediator;

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var command = new SignInCommand { Email = Email, Password = Password };
            var user = await _mediator.Send(command);
            System.Console.WriteLine($"Check log user: {user.ToJson()}");
            ErrorMessage = $"Logged in as Verification code sent.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}