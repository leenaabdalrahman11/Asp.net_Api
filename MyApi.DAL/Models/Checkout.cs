using System;

namespace MyApi.DAL.Models;

internal class Checkout
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? ProductId { get; set; }
    public DateTime CheckoutDate { get; set; }
}
