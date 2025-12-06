using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Response
{
    public class CategoryTranslationResponse
    {
        public string? Name { get; set; }
        public string Language { get; set; } = "en";

    }
}
