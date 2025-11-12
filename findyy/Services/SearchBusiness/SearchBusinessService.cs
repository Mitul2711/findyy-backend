using findyy.DTO;
using findyy.Model.Response;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Repository.SearchBusiness.Interface;
using findyy.Services.SearchBusiness.Interface;

namespace findyy.Services.SearchBusiness
{
    public class SearchBusinessService : ISearchBusinessService
    {
        private readonly ISearchBusinessRepository _repo;

        public SearchBusinessService(ISearchBusinessRepository repo)
        {
            _repo = repo;
        }

        public async Task<Response> SearchBusinessesAsync(SearchBusinessDTO data)
        {
            var businesses = await _repo.SearchBusinessesAsync(data);
            if (businesses == null)
            {

                return new Response
                {
                    Status = false,
                    Message = "Businesses Not Found",
                    Data = businesses
                };
            }
            return new Response
            {
                Status = true,
                Message = "Businesses retrieved successfully",
                Data = businesses
            };
        }
    }
}
