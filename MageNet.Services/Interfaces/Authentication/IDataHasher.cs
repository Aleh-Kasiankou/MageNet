namespace MageNetServices.Interfaces.Authentication;

public interface IDataHasher
{
    string HashData(string data);
}