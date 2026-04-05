using System;
using System.Text.Json.Serialization;
using MyApi.DAL.Models;

namespace MyApi.DAL.DTO.Response;

public class ProductResponse
{
        public int Id { get; set; }
        //public int CategoryId { get; set; } 
        [JsonConverter(typeof(JsonStringEnumConverter))]// This attribute ensures that the enum is serialized as a string in JSON
        public Status Status { get; set; }
        public string CreatedBy {get;set;}
        public string MainImage { get; set; }
        public List<CategoryTranslationResponse>? Translations { get; set; }
}
