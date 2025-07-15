namespace ComputerStoreContracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, byte[] fileData, string fileName);
}
