using System;

namespace MyApi.DAL.DTO.Requests;

public class ProductTranslationRequest
{
    public string Language { get; set; } = "en";
    public string Name { get; set; }
    public string Description { get; set; }

}
