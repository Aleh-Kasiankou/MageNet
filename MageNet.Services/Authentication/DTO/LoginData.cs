using MageNetServices.Interfaces.Authentication;

namespace MageNetServices.Authentication.DTO;

public class LoginData : ILoginData
{
    public string UserName { get; set; }
    public string Password { get; set; }
}