namespace WebApplication1.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        try
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("siddiqjonovsaidabrorr@gmail.com", "15-latter code should be here")
            };

            var message = new MailMessage("siddiqjonovsaidabrorr@gmail.com", request.ToEmail, request.Subject, request.Body);
            await client.SendMailAsync(message);

            return Ok("Email sent successfully");
        }
        catch (SmtpException ex)
        {
            return BadRequest($"Failed to send email: {ex.Message}");
        }
    }
}

public class EmailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
