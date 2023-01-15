using MageNetServices.Authentication.DTO;

namespace MageNetServices.Interfaces.Authentication;

public interface ILoginHandler
{
    Task<(bool isAuthSuccessful, string? token)> TryLogInBackendUser(ILoginData loginData);
}