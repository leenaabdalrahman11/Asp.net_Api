namespace MyApi.PLL.Models
{
    public class BaseModel
    {
        public int Id { get; set; } 
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
