using System;

namespace MyApi.DAL.Models;

public class Reviews
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
