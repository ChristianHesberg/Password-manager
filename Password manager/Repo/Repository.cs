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
}   