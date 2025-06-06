﻿namespace CustodyWallet.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
}