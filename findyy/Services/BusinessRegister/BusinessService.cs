using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Model.Response;
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
        public async Task<Response> GetBusinessAsync(Guid id)
        {
            var business = await _repo.GetBusinessAsync(id);
            if (business == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Business not found",
                    Data = null
                };
            }

            return new Response
            {
                Status = true,
                Message = "Business retrieved successfully",
                Data = business
            };
        }

        public async Task<Response> GetAllBusinessesAsync()
        {
            var businesses = await _repo.GetAllBusinessesAsync();
            return new Response
            {
                Status = true,
                Message = "Businesses retrieved successfully",
                Data = businesses
            };
        }

        public async Task<Response> CreateBusinessAsync(BusinessDto dto)
        {
            var id = await _repo.CreateBusinessAsync(dto);
            return new Response
            {
                Status = true,
                Message = "Business created successfully",
                Data = id
            };
        }

        public async Task<Response> UpdateBusinessAsync(BusinessUpdateDto dto)
        {
            await _repo.UpdateBusinessAsync(dto);
            return new Response
            {
                Status = true,
                Message = "Business updated successfully",
                Data = null
            };
        }

        public async Task<Response> DeleteBusinessAsync(long id)
        {
            await _repo.DeleteBusinessAsync(id);
            return new Response
            {
                Status = true,
                Message = "Business deleted successfully",
                Data = null
            };
        }

        // ------------------- Location -------------------
        public async Task<Response> GetBusinessLocationAsync(long id)
        {
            var location = await _repo.GetBusinessLocationAsync(id);
            if (location == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Business location not found",
                    Data = null
                };
            }

            return new Response
            {
                Status = true,
                Message = "Business location retrieved successfully",
                Data = location
            };
        }

        public async Task<Response> CreateOrUpdateLocationAsync(BusinessLocationDto dto)
        {
            var action = dto.Id == 0 ? "POST" : "PUT";
            await _repo.CreateOrUpdateLocationAsync(dto, action);

            return new Response
            {
                Status = true,
                Message = dto.Id == 0 ? "Location created successfully" : "Location updated successfully",
                Data = null
            };
        }

        // ------------------- Hours -------------------    
        public async Task<Response> GetBusinessHoursAsync(long businessId)
        {
            var hours = await _repo.GetBusinessHoursAsync(businessId);
            return new Response
            {
                Status = true,
                Message = "Business hours retrieved successfully",
                Data = hours
            };
        }

        public async Task<Response> CreateOrUpdateBusinessHourAsync(List<BusinessHourDto> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return new Response
                {
                    Status = false,
                    Message = "No business hours provided",
                    Data = null
                };
            }

            var hasExisting = dtos.Any(x => x.Id != 0);
            var action = hasExisting ? "PUT" : "POST";

            await _repo.CreateOrUpdateBusinessHourAsync(dtos, "BULK_SAVE");

            return new Response
            {
                Status = true,
                Message = hasExisting ? "Business hours updated successfully" : "Business hours created successfully",
                Data = null
            };
        }

        // ------------------- Verify -------------------
        public async Task<Response> ReviewBusinessAsync(long businessId, bool isVerified, byte status)
        {
            await _repo.ReviewBusinessAsync(businessId, isVerified, status);

            return new Response
            {
                Status = true,
                Message = "Business verified successfully",
                Data = null
            };
        }

    }
}
