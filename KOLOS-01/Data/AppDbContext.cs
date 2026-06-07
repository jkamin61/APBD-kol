using Microsoft.EntityFrameworkCore;

namespace KOLOS_01.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}