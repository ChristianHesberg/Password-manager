using System.Security.Cryptography;
using System.Text;
using Password_manager.Entities;
using Password_manager.Repo;
using Password_manager.Utils;

namespace Password_manager.Services;

public class VaultService
{
    private readonly Repository _repo;
    private readonly EncryptionDecryptionUtils _encryptionDecryptionUtils;

    public VaultService(Repository repo, EncryptionDecryptionUtils encryptionDecryptionUtils)
    {
        _repo = repo;
        _encryptionDecryptionUtils = encryptionDecryptionUtils;
    }

    public List<Entry> GetEntries(byte[] key)
    {
        var items = _repo.GetVaultItems();
        var decryptedItems = new List<Entry>();
        foreach (var item in items)
        {
            var decrypted = _encryptionDecryptionUtils.Decrypt(
                item.Data,
                item.Nonce,
                item.Tag,
                key
            );
            var deserialized = _encryptionDecryptionUtils.ExtractPartsFromData(decrypted);
            decryptedItems.Add(new Entry()
            {
                Password = deserialized.password,
                Url = deserialized.url
            });
        }

        return decryptedItems;
    }

    public void AddEntry(Entry entity, byte[] key)
    {
        var serializedData = _encryptionDecryptionUtils.SerializePayload(entity.Url, entity.Password);
        var encrypted = _encryptionDecryptionUtils.Encrypt(serializedData, key);
        var vaultItem = new VaultItem()
        {
            Data = encrypted.ciphertext,
            Nonce = encrypted.nonce,
            Tag = encrypted.tag
        };
        _repo.AddVaultItem(vaultItem);
    }


}