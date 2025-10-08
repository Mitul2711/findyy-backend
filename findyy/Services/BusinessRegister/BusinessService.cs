using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Services.BusinessRegister.Interface;

namespace findyy.Services.BusinessRegister
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _repo;

        public BusinessService(IBusinessRepository repo)
        {
            _repo = repo;
        }

        // ------------------- Business -------------------
        public async Task<Business?> GetBusinessAsync(long id)
        {
            return await _repo.GetBusinessAsync(id);
        }

        public async Task<List<Business>> GetAllBusinessesAsync()
        {
            return await _repo.GetAllBusinessesAsync();
        }

        public async Task<long> CreateBusinessAsync(BusinessDto dto)
        {
            return await _repo.CreateBusinessAsync(dto);
        }

        public async Task UpdateBusinessAsync(BusinessUpdateDto dto)
        {
            await _repo.UpdateBusinessAsync(dto);
        }

        public async Task DeleteBusinessAsync(long id)
        {
            await _repo.DeleteBusinessAsync(id);
        }

        // ------------------- Location -------------------
        public async Task<BusinessLocation?> GetBusinessLocationAsync(long id)
        {
            return await _repo.GetBusinessLocationAsync(id);
        }

        public async Task CreateOrUpdateLocationAsync(BusinessLocationDto dto, string action)
        {
            await _repo.CreateOrUpdateLocationAsync(dto, action);
        }

        // ------------------- Hours -------------------
        public async Task<List<BusinessHour>> GetBusinessHoursAsync(long businessId)
        {
            return await _repo.GetBusinessHoursAsync(businessId);
        }

        public async Task CreateOrUpdateBusinessHourAsync(BusinessHourDto dto, string action)
        {
            await _repo.CreateOrUpdateBusinessHourAsync(dto, action);
        }
    }
}
