using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.DTO.Response
{
 public class ForgotPasswordResponse : BaseResponse
    {
        public string? AccessToken { get; set; }
    }   
}
