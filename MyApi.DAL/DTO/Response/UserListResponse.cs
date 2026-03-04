using System;

namespace MyApi.DAL.DTO.Response;

public class UserListResponse
{
    public string Id {get;set;}
    public string FullName {get;set;}

    public bool IsBlocked {get;set;}
    public List<string> Roles {get;set;}
    
}
