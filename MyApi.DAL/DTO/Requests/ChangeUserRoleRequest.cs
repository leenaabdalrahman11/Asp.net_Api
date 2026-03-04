using System;

namespace MyApi.DAL.DTO.Requests;

public class ChangeUserRoleRequest
{
    public string UserId {get;set;}
    public string? Role {get;set;} = string.Empty;

}
