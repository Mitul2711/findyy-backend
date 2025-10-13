using findyy.Model.Response;
using findyy.Repository.BusinessDash.Interface;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Services.BusinessDash.Interface;

namespace findyy.Services.BusinessDash
{
    public class BusinessDashService: IBusinessDashService
    {

        private readonly IBusinessDashRepository _repo;

        public BusinessDashService(IBusinessDashRepository repo)
        {
            _repo = repo;
        }
        public async Task<Response> GetBusinessProfileCompletionAsync(long businessId)
        {
            var result = await _repo.GetBusinessProfileCompletionAsync(businessId);

            if (result == null)
                return new Response
                {
                    Status = false,
                    Message = "Business not found",
                    Data = null
                };

            return new Response
            {
                Status = true,
                Message = "Business profile completion retrieved successfully",
                Data = result
            };
        }

    }
}
