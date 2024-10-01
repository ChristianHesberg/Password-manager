﻿using System.ComponentModel.DataAnnotations;

namespace Password_manager.Entities;

public class VaultItem
{
    [Key]
    public string Url { get; set; }
    public string EncryptedPassword { get; set; }
    public string Nonce { get; set; }
    public string Tag { get; set; }
}