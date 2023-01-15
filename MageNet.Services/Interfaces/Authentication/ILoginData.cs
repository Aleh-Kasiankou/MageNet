namespace MageNetServices.Interfaces.Authentication;

public interface ILoginData
{
    public string UserName { get; set; }
    public string Password { get; set; }

}