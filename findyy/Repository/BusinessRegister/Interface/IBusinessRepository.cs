using findyy.DTO.Business;
using findyy.Model.BusinessRegister;

namespace findyy.Repository.BusinessRegister.Interface
{
    public interface IBusinessRepository
    {
        Task<Business?> GetBusinessAsync(long id);
        Task<List<Business>> GetAllBusinessesAsync();
        Task<long> CreateBusinessAsync(BusinessDto dto);
        Task UpdateBusinessAsync(BusinessUpdateDto dto);
        Task DeleteBusinessAsync(long id);

        // ------------------- Business Location -------------------
        Task<BusinessLocation?> GetBusinessLocationAsync(long id);
        Task CreateOrUpdateLocationAsync(BusinessLocationDto dto, string action);

        // ------------------- Business Hours -------------------
        Task<List<BusinessHour>> GetBusinessHoursAsync(long businessId);
        Task CreateOrUpdateBusinessHourAsync(BusinessHourDto dto, string action);
    }
}
