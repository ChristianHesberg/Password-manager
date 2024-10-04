using System.ComponentModel.DataAnnotations;

namespace Password_manager.Entities;

public class VaultItem
{
    [Key]
    public byte[] Data { get; set; }
    public byte[] Nonce { get; set; }
    public byte[] Tag { get; set; }
}