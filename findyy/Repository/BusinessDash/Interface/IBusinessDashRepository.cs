using findyy.DTO.BusinessDash;
using findyy.Model.BusinessRegister;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Repository.BusinessDash.Interface
{
    public interface IBusinessDashRepository
    {
        Task<BusinessDashboardDto?> GetBusinessProfileCompletionAsync(long businessId);

    }
}
