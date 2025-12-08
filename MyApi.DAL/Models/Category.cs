using MyApi.DAL.Models;

namespace MyApi.DAL.Models
{
    public class Category: BaseModel
    {
        public List<CategoryTranslation>? Translations { get; set; }
    }
}
