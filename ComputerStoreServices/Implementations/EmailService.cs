using ComputerStoreContracts.Services;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ComputerStoreServices.Implementations;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("nsshipilov@gmail.com", "pftn icnc mmcf jyun"),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string email, string subject, byte[] pdfData, string fileName)
    {
        var message = new MailMessage(
            from: "nsshipilov@gmail.com",
            to: email,
            subject: subject,
            body: "Прикреплен отчет в формате PDF"
        )
        {
            IsBodyHtml = false
        };

        using var ms = new MemoryStream(pdfData);
        var attachment = new Attachment(ms, fileName, MediaTypeNames.Application.Pdf);
        message.Attachments.Add(attachment);

        await _smtpClient.SendMailAsync(message);
    }
}