using findyy.DTO.Auth;
using findyy.Model.Response;
using findyy.Services.Auth.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using findyy.Services.Admin.Interface;

namespace findyy.Services.Admin
{
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AdminNotificationService(IEmailService emailService, IConfiguration config)
        {
            _emailService = emailService;
            _config = config;
        }

        public async Task<Response> NotifyNewBusinessAsync(notificationDTO dto)
        {
            try
            {
                var adminEmail = _config["Smtp:AdminEmail"];
                if (string.IsNullOrEmpty(adminEmail))
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Admin email not configured",
                        Data = null
                    };
                }

                var body = $@"
                    <h3>New Business Registration Request</h3>
                    <p>A new business has registered on the platform.</p>
                    <ul>
                        <li>Name: {dto.FirstName} {dto.LastName}</li>
                        <li>Business Name: {dto.BusinessName}</li>
                        <li>Business Description: {dto.BusinessDescription}</li>
                        <li>Email: {dto.Email}</li>
                    </ul>";

                await _emailService.SendEmailAsync(adminEmail, "New Business Registration Request", body);

                return new Response
                {
                    Status = true,
                    Message = "Admin notified successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Failed to notify admin: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<Response> NotifyToBusinessAsync(notificationDTO dto)
        {
            try
            {
                var adminEmail = dto.Email;
                if (string.IsNullOrEmpty(adminEmail))
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Business email not configured",
                        Data = null
                    };
                }

                var body = $@"
                    <h3>🎉 Your Business Has Been Verified!</h3>
                    <p>Great news! {dto.FirstName} {dto.LastName}, your business <strong>{dto.BusinessName}</strong> has been successfully verified and is now live on our platform.</p>
                    <p>Here’s a quick summary of your business details:</p>
                    <p>{dto.BusinessDescription}</p>
                    <p>If you need to update any information, you can do so anytime through your account. Thank you for being part of our community—we're excited to support you as your business grows!</p>
                    <p>– The localbiz Team</p>";

                await _emailService.SendEmailAsync(adminEmail, "New Business Registration Request", body);

                return new Response
                {
                    Status = true,
                    Message = "Business Owner notified successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Failed to notify Business Owner: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
