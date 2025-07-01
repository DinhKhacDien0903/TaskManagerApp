using Application.Common.Interfaces;
using MediatR;

namespace Application.Author.Commands;

public record SignUpCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class SignUpCommandHandler(IAuthorService authorService) : IRequestHandler<SignUpCommand, string>
{
    private readonly IAuthorService _authorService = authorService;

    public async Task<string> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var (result, token) = await _authorService.SignUpAsync(request.Email, request.Password);
        if (result.Succeeded)
        {
            return token;
        }

        return result.Errors.FirstOrDefault() ?? "An error occurred during sign-up.";
    }
}