using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Response
{
    public class RegisterResponse : BaseResponse
    {
        public string? UserId { get; set; }           // new user id when succeeded
    }
}