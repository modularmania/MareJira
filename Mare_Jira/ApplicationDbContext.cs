using Microsoft.EntityFrameworkCore;

namespace MareJira;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; } = null!;
}
public class UserEntity
{
    public ulong Id { get; set; }
    public string Name { get; set; }
}
