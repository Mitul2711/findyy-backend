using findyy.DTO.Business;
using findyy.Model.Response;

namespace findyy.Services.BusinessReview.Interface
{
    public interface IBusinessReviewService
    {
        Task<Response> CreateReviewAsync(BusinessReviewDTO dto);
        Task<Response> GetAllOrByIdReviewForUserAsync(Guid? UserId = null);
        Task<Response> GetAllOrByIdReviewForBusinessAsync(long? UserId = null);
    }
}
