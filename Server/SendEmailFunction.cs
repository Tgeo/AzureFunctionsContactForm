using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Server
{
    /// <summary>
    /// See SendEmailFunction.CSX for what the actual file you'd upload to Azure Functions would look like.
    /// </summary>
    public static class SendEmailFunction
    {
        private static readonly string _sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
        private static readonly string _toEmail = Environment.GetEnvironmentVariable("TO_EMAIL");

        /// <summary>
        /// The code that is run by the Azure function.
        /// </summary>
        /// <remarks>
        /// Some annoyances: Azure Functions expect a ".csx" file -- which is a C# "script" file.
        /// Intellisense support for these files is lacking, so this is just part of a class for now
        /// and the body of the Run() method is manually pasted into the Function app via Azure portal.
        /// </remarks>
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req)
        {
            if (req.Method != HttpMethod.Post)
                return req.CreateResponse(HttpStatusCode.MethodNotAllowed, "Only POST is supported");  
                      
            var postBody = await req.Content.ReadAsAsync<SendEmailPostBody>();            
            if (!postBody.IsValid)
                return req.CreateResponse(HttpStatusCode.BadRequest);

            var sendGridMessage = new SendGridMessage();
            sendGridMessage.From = new MailAddress(postBody.Email);
            sendGridMessage.AddTo(_toEmail);
            sendGridMessage.Subject = "An email from your contact form";
            sendGridMessage.Html = $"Name: {postBody.Name}<br>" +
                $"Email: {postBody.Email}<br><br>" +
                postBody.Text;
            
            var sendGridWebTransport = new Web(_sendGridApiKey);
            await sendGridWebTransport.DeliverAsync(sendGridMessage);
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class SendEmailPostBody
    {
        private readonly Regex _emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Name) &&
                    !string.IsNullOrEmpty(Email) &&
                    _emailRegex.IsMatch(Email) &&
                    !string.IsNullOrEmpty(Text);
            }
        }
    }
}
