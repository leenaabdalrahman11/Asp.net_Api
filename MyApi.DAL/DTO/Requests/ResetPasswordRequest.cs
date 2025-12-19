using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.DAL.DTO.Response;

namespace MyApi.DAL.DTO.Requests
{
    public class ResetPasswordRequest 
{
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ResetCode { get; set; }

    
}
}
