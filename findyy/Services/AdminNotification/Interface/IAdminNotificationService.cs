using findyy.DTO.Auth;
using findyy.Model.Response;
using System.Threading.Tasks;

namespace findyy.Services.Admin.Interface
{
    public interface IAdminNotificationService
    {
        Task<Response> NotifyNewBusinessAsync(notificationDTO dto);
        Task<Response> NotifyToBusinessAsync(notificationDTO dto);
    }
}
