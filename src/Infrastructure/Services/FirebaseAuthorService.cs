using Application.Common.Interfaces;
using Application.Common.Models;
using Firebase.Auth;

namespace Infrastructure.Services;

public class FirebaseAuthorService(FirebaseAuthClient authClient) : IAuthorService
{
    private readonly FirebaseAuthClient _authClient = authClient;
    public async Task<(Result result, string UserId)> SignInAsync(string email, string password)
    {
        try
        {
            var authResponse = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            if (authResponse.User == null)
                return (Result.Failure(["Invalid email or password."]), string.Empty);

            var idToken = await authResponse.User.GetIdTokenAsync();
            return (Result.Success(), idToken);
        }
        catch (FirebaseAuthException ex)
        {
            return (Result.Failure([ex.Message]), string.Empty);
        }
        catch (Exception ex)
        {
            return (Result.Failure([ex.Message]), string.Empty);
        }
    }

    public async Task<(Result result, string Token)> SignUpAsync(string email, string password)
    {
        try
        {
            var authResponse = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
            if (authResponse.User == null)
                return (Result.Failure(["Failed to create user."]), string.Empty);

            var idToken = await authResponse.User.GetIdTokenAsync();
            return (Result.Success(), idToken);
        }
        catch (FirebaseAuthException ex)
        {
            return (Result.Failure([ex.Message]), string.Empty);
        }
        catch (Exception ex)
        {
            return (Result.Failure([ex.Message]), string.Empty);
        }
    }
}