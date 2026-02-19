using MyApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Response
{
    public class CategoryResponse
    {

        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string CreatedBy {get;set;}
        public List<CategoryTranslationResponse>? Translations { get; set; }
        public object Name { get; set; }
    }
}
