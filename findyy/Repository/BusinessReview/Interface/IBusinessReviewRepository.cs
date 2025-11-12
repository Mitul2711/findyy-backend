using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Model.BusinessReview;

namespace findyy.Repository.BusinessReviewRepo.Interface
{
    public interface IBusinessReviewRepository
    {
        Task<long> CreateReviewAsync(BusinessReviewDTO dto);
        Task<List<BusinessReview>> GetAllOrByIdReviewForUserAsync(Guid? UserId = null);
        Task<List<BusinessReview>> GetAllOrByIdReviewForBusinessAsync(long? Id = null);
    }
}
