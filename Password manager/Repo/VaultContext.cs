using Microsoft.EntityFrameworkCore;
using Password_manager.Entities;

namespace Password_manager.Repo;

public class VaultContext : DbContext
{
    public DbSet<VaultItem> Vault { get; set; }

    public DbSet<PasswordHash> Password { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databasePath = "../../../passwordmanager.db";  
        optionsBuilder.UseSqlite($"Data Source={databasePath}");  
        
        Console.WriteLine($"Database Path: {databasePath}"); 
    }
}