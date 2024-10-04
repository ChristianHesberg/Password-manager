using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Password_manager.Entities;

namespace Password_manager.Repo;

public class Repository
{
    private readonly VaultContext db;

    public Repository(VaultContext db)
    {
        this.db = db;
    }

    public void AddPasswordHash(PasswordHash entity)
    {
        db.Add(entity);
        db.SaveChanges();
    }

    public PasswordHash GetPasswordHash()
    {
        return db.Password.ToList().First();
    }

    public void AddVaultItem(VaultItem entity)
    {
        db.Add(entity);
        db.SaveChanges();
    }

    public List<VaultItem> GetVaultItems()
    {
        return db.Vault.ToList();
    }

    public void ClearDatabase()
    {
        var entity1 = db.Password.First();
        db.Password.Remove(entity1);
        
        var entity2 = db.Vault.First();
        db.Vault.Remove(entity2);
        db.SaveChanges();
    } 
}   