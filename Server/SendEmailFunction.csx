// This is manually copied/manipulated from SendEmailFunction.CS because this is a script file (CSX).
// This is the file you'd actually upload to Azure Functions.
// TODO: Automated the copy/manpilation.

using System.Net;
using SendGrid;
using System.Net.Mail;
using System.Text.RegularExpressions;

private static readonly string _sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
private static readonly string _toEmail = Environment.GetEnvironmentVariable("TO_EMAIL");

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    if (req.Method != HttpMethod.Post)
        return req.CreateResponse(HttpStatusCode.MethodNotAllowed, "Only POST is supported");

    try
    {
        // Request body must be of Content-Type: application/json. (Azure Functions limitation).
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
    catch (Exception ex)
    {
        log.Info(ex.ToString());
        return req.CreateResponse(HttpStatusCode.InternalServerError);
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