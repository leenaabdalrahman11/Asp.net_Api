using System;

namespace MyApiProject.MyApi.DAL.DTO.Response;

public class ErrorDetails
{
    public string? Message { get; set; }
    public int? StatusCode { get; set; }
    public string? StackTrace { get; set; }



}
