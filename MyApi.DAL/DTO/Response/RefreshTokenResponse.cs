using System;

namespace MyApi.DAL.DTO.Response;

public class RefreshTokenResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }

}
