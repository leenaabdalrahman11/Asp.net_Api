using System;

namespace MyApi.DAL.DTO.Response;

public class ProductTranslationResponse
{
    public string Language { get; set; } = "en";
    public string Name { get; set; }
    public string Description { get; set; }
}
