using Microsoft.EntityFrameworkCore;

namespace MageNet.Persistence;

public class MageNetDbContext : DbContext
{
    public MageNetDbContext(DbContextOptions<MageNetDbContext> options) : base(options)
    {
        
    }
}