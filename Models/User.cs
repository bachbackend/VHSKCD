using System;
using System.Collections.Generic;

namespace VHSKCD.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public sbyte Status { get; set; }

    public string Phonenumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ResetToken { get; set; }

    public string? ResetTokenExpired { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
