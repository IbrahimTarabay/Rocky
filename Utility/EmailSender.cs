using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email,subject,htmlMessage);
        }

        public async Task Execute(string email, string subject, string body) {
            MailjetClient client = new MailjetClient("dbd8c7e587730562bdb11797901adb55", "ed40906ebb039948d853ed7362382c9a");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "ibrahimesalah69@gmail.com"},
        {"Name", "Ibrahim"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Ibrahim"
         }
        }
       }
      }, {
       "Subject",
       subject
      },
      {
       "HTMLPart",
        body
       }
     }
             });
        await client.PostAsync(request);
        }
    }
}
