using System;

namespace MyApi.DAL.DTO.Requests;

public class TokenApiModelRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

}
