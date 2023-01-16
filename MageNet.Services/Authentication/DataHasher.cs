using System.Security.Cryptography;
using System.Text;
using MageNetServices.Interfaces.Authentication;
using Microsoft.Extensions.Options;

namespace MageNetServices.Authentication;

public class DataHasher : IDataHasher
{
    private readonly byte[] _salt;
    private static readonly HashAlgorithm HashAlgorithm = SHA256.Create();

    public DataHasher(IOptions<JwtSecurityKey> options)
    {
        _salt = Encoding.UTF8.GetBytes(options.Value.Salt);
    }

    public string HashData(string data)
    {
        Byte[] inputBytes = Encoding.UTF8.GetBytes(data);

        // Combine salt and input bytes
        Byte[] saltedInput = new Byte[_salt.Length + inputBytes.Length];
        _salt.CopyTo(saltedInput, 0);
        inputBytes.CopyTo(saltedInput, _salt.Length);


        Byte[] hashedBytes = HashAlgorithm.ComputeHash(saltedInput);

        return Convert.ToBase64String(hashedBytes);
    }
}