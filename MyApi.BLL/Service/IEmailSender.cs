using System.Threading.Tasks;

namespace MyApi.BLL.Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}