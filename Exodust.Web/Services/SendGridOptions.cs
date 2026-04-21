namespace Exodust.Web.Services;

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = "noreply@exodust.local";
    public string FromName { get; set; } = "Exodust";
}
