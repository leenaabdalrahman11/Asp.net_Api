using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Requests
{
    public class CategoryRequest
    {
        public required List<CategoryTranslationRequest> Translations { get; set; }
    }
}
