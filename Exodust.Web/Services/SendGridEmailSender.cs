using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Exodust.Web.Services;

public class SendGridEmailSender(
    IOptions<SendGridOptions> options,
    ILogger<SendGridEmailSender> logger) : IEmailSender
{
    private readonly SendGridOptions _options = options.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            logger.LogWarning("SendGrid ApiKey is not configured. Skipping email to {Email}.", email);
            return;
        }

        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress(_options.FromEmail, _options.FromName);
        var to = new EmailAddress(email);
        var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlMessage);
        await client.SendEmailAsync(message);
    }
}
