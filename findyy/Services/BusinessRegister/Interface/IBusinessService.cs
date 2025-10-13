using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Model.Response;

namespace findyy.Services.BusinessRegister.Interface
{
    public interface IBusinessService
    {
        // Business
        Task<Response> GetBusinessAsync(Guid id);
        Task<Response> GetAllBusinessesAsync();
        Task<Response> CreateBusinessAsync(BusinessDto dto);
        Task<Response> UpdateBusinessAsync(BusinessUpdateDto dto);
        Task<Response> DeleteBusinessAsync(long id);

        // Location
        Task<Response> GetBusinessLocationAsync(long id);
        Task<Response> CreateOrUpdateLocationAsync(BusinessLocationDto dto);

        // Hours
        Task<Response> GetBusinessHoursAsync(long businessId);
        Task<Response> CreateOrUpdateBusinessHourAsync(List<BusinessHourDto> dtos);

        Task<Response> ReviewBusinessAsync(long businessId, bool isVerified, byte status);

    }
}
