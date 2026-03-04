using System;

namespace MyApi.DAL.DTO.Response;

public class ReviewResponse
{
    public string FullName {get;set;}
    public int Rating {get;set;}
    public string Comment {get;set;}
    public DateTime CreatedAt { get; set; }

}
