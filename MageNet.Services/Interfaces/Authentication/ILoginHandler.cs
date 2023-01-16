using MageNetServices.Authentication.DTO;

namespace MageNetServices.Interfaces.Authentication;

public interface ILoginHandler
{
    Task<(bool IsAuthSuccessful, string? Token)> TryLogInBackendUser(ILoginData loginData);
}