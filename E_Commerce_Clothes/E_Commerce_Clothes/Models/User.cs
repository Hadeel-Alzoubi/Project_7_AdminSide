﻿using System;
using System.Collections.Generic;

namespace E_Commerce_Clothes.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Image { get; set; }

    public string Password { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? Address { get; set; }

    public int? Points { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Copon> Copons { get; set; } = new List<Copon>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
