using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MageNet.Persistence;
using MageNetServices.Interfaces.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MageNetServices.Authentication;

public class JwtLoginHandler : ILoginHandler
{
    private readonly MageNetDbContext _dbContext;
    private readonly IDataHasher _dataHasher;
    private readonly SecurityKey _key;

    public JwtLoginHandler(MageNetDbContext dbContext, IDataHasher dataHasher, IOptions<JwtSecurityKey> keyConfig)
    {
        _dbContext = dbContext;
        _dataHasher = dataHasher;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyConfig.Value.Key));
    }

    public async Task<(bool IsAuthSuccessful, string? Token)> TryLogInBackendUser(ILoginData loginData)
    {
        var user = await _dbContext.BackendUsers.SingleOrDefaultAsync(x => x.UserName == loginData.UserName);

        if (user != null)
        {
            var isAuthSuccessful = _dataHasher.HashData(loginData.Password) == user.PasswordHash;

            if (isAuthSuccessful)
            {
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Expires = (DateTime.Today.AddDays(1)),
                    SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return (isAuthSuccessful, tokenHandler.WriteToken(token));
            }
        }

        return (IsAuthSuccessful: false, Token: null);
    }
}