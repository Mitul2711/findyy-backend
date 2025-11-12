using findyy.DTO.Business;
using findyy.Model.Response;
using findyy.Repository.BusinessReviewRepo.Interface;
using findyy.Services.BusinessReview.Interface;

namespace findyy.Services.BusinessReview
{
    public class BusinessReviewService : IBusinessReviewService
    {
        private readonly IBusinessReviewRepository _repo;

        public BusinessReviewService(IBusinessReviewRepository repo)
        {
            _repo = repo;
        }

        public async Task<Response> CreateReviewAsync(BusinessReviewDTO dto)
        {
            var id = await _repo.CreateReviewAsync(dto);
            return new Response
            {
                Status = true,
                Message = "Review submitted successfully",
                Data = id
            };
        }

        public async Task<Response> GetAllOrByIdReviewForUserAsync(Guid? UserId = null)
        {
            var businesses = await _repo.GetAllOrByIdReviewForUserAsync(UserId);
            return new Response
            {
                Status = true,
                Message = "Business Reviews retrieved successfully",
                Data = businesses
            };
        }

        public async Task<Response> GetAllOrByIdReviewForBusinessAsync(long? UserId = null)
        {
            var businesses = await _repo.GetAllOrByIdReviewForBusinessAsync(UserId);
            return new Response
            {
                Status = true,
                Message = "Business Reviews retrieved successfully",
                Data = businesses
            };
        }
    }
}
