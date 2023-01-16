namespace MageNet.Persistence.Models;

public class BackendUser
{
    public Guid BackendUserId { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}