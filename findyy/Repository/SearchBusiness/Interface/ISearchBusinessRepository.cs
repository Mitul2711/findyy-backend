using findyy.DTO;
using findyy.Model.BusinessRegister;

namespace findyy.Repository.SearchBusiness.Interface
{
    public interface ISearchBusinessRepository
    {
        Task<List<BusinessSearchResultDto>> SearchBusinessesAsync(SearchBusinessDTO data);
    }
}
