using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace MyApi.BLL.Service;

public class EmailSender : IEmailSender
{
  public Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    var client = new SmtpClient("smtp.gmail.com", 587)
    {
      EnableSsl = true,
      UseDefaultCredentials = false,
      Credentials = new NetworkCredential(
                  "leenasa272@gmail.com",
                  "xmbn mfos ykvr uzzg"
              )
    };

    var mailMessage = new MailMessage(
        from: "leenasa272@gmail.com",
        to: email,
        subject: subject,
        body: htmlMessage
    )
    {
      IsBodyHtml = true
    };

    return client.SendMailAsync(mailMessage);
  }

}