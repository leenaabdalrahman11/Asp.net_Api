namespace MyApi.PLL.Models
{
    public class Category: BaseModel
    {
        public List<CategoryTranslation>? Translations { get; set; }
    }
}
