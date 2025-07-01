using Application.Common.Interfaces;
using MediatR;

namespace Application.Author.Commands;

public record SignInCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class SignInCommandHandler(IAuthorService authorService) : IRequestHandler<SignInCommand, string>
{
    private readonly IAuthorService _authorService = authorService;

    public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var (result, userId) = await _authorService.SignInAsync(request.Email, request.Password);
        if (result.Succeeded)
        {
            return userId;
        }

        return result.Errors.FirstOrDefault() ?? "An error occurred during sign-in.";
    }
}