using findyy.Model.Response;

namespace findyy.Services.BusinessDash.Interface
{
    public interface IBusinessDashService
    {
        Task<Response> GetBusinessProfileCompletionAsync(long businessId);
    }
}
