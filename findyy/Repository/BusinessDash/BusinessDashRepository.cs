using findyy.DTO.BusinessDash;
using findyy.Repository.BusinessDash.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace findyy.Repository.BusinessDash
{
    public class BusinessDashRepository: IBusinessDashRepository
    {

        private readonly AppDbContext _db;
        public BusinessDashRepository(AppDbContext db) => _db = db;
        public async Task<BusinessDashboardDto?> GetBusinessProfileCompletionAsync(long businessId)
        {
            await using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetBusinessProfileCompletion";
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.CreateParameter();
            param.ParameterName = "@BusinessId";
            param.Value = businessId;
            cmd.Parameters.Add(param);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new BusinessDashboardDto
                {
                    CompletionPercentage = reader.GetInt32(reader.GetOrdinal("CompletionPercentage")),
                    IsVerified = reader.GetBoolean(reader.GetOrdinal("IsVerified"))
                };
            }

            return null;
        }

    }
}
