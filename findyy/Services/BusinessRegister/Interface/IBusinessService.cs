using findyy.DTO.Business;
using findyy.Model.BusinessRegister;

namespace findyy.Services.BusinessRegister.Interface
{
    public interface IBusinessService
    {
        // Business
        Task<Business?> GetBusinessAsync(long id);
        Task<List<Business>> GetAllBusinessesAsync();
        Task<long> CreateBusinessAsync(BusinessDto dto);
        Task UpdateBusinessAsync(BusinessUpdateDto dto);
        Task DeleteBusinessAsync(long id);

        // Location
        Task<BusinessLocation?> GetBusinessLocationAsync(long id);
        Task CreateOrUpdateLocationAsync(BusinessLocationDto dto, string action);

        // Hours
        Task<List<BusinessHour>> GetBusinessHoursAsync(long businessId);
        Task CreateOrUpdateBusinessHourAsync(BusinessHourDto dto, string action);

    }
}
