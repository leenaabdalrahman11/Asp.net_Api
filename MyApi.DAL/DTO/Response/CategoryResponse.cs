using MyApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Response
{
    public class CategoryResponse
    {

        public int Id { get; set; }
        public string UserCreated {get;set;}
        public Status Status { get; set; }
        public List<CategoryTranslationResponse>? Translations { get; set; }
    }
}
