using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace MyApi.DAL.DTO.Response
{
public class RegisterResponse
{
    public bool IsSuccess { get; set; }          // true when registration succeeded
    public string? Message { get; set; }          // short human message
    public IEnumerable<string>? Errors { get; set; } // detailed errors (if any)
    public string? UserId { get; set; }           // new user id when succeeded
}
}