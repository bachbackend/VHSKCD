using System;
using System.Collections.Generic;

namespace VHSKCD.Models;

public partial class Article
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string Thumbnail { get; set; } = null!;

    public sbyte? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UserId { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string Content { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
