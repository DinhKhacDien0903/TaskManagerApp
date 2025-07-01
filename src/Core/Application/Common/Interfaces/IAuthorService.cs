using Application.Common.Models;
namespace Application.Common.Interfaces;

public interface IAuthorService
{
    Task<(Result result, string UserId)> SignInAsync(string email, string password);
    Task<(Result result, string Token)> SignUpAsync(string email, string password);
}