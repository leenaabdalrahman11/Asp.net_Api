namespace MyApi.PLL.Models
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string Language { get; set; } = "en";

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
