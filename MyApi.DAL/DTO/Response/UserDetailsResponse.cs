using System;

namespace MyApi.DAL.DTO.Response;

public class UserDetailsResponse
{
        public string Id {get;set;}
    public string FullName {get;set;}
    public string Email {get;set;}
    public string PhoneNumber {get;set;}
    public string City {get;set;}
    public bool EmailConfirmed {get;set;}
    public bool IsBlocked {get;set;}
    public List<string> Roles {get;set;}
}
