using findyy.DTO;
using findyy.Model.Response;

namespace findyy.Services.SearchBusiness.Interface
{
    public interface ISearchBusinessService
    {
        Task<Response> SearchBusinessesAsync(SearchBusinessDTO data);
    }
}
