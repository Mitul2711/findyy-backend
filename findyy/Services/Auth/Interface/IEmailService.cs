namespace findyy.Services.Auth.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

}
