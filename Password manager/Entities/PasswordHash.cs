using System.ComponentModel.DataAnnotations;

namespace Password_manager.Entities;

public class PasswordHash
{
    [Key]
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}