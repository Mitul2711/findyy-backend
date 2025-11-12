using findyy.DTO;
using findyy.Model.BusinessRegister;
using findyy.Repository.SearchBusiness.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace findyy.Repository.SearchBusiness
{
    public class SearchBusinessRepository : ISearchBusinessRepository 
    {
        private readonly AppDbContext _db;
        public SearchBusinessRepository(AppDbContext db) => _db = db;
        public async Task<List<BusinessSearchResultDto>> SearchBusinessesAsync(SearchBusinessDTO data)
        {
            var result = new List<BusinessSearchResultDto>();

            await using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_SearchBusinesses";
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters safely
            var categoryParam = cmd.CreateParameter();
            categoryParam.ParameterName = "@Category";
            categoryParam.Value = (object?)data.Category ?? DBNull.Value;
            cmd.Parameters.Add(categoryParam);

            var locationParam = cmd.CreateParameter();
            locationParam.ParameterName = "@Location";
            locationParam.Value = (object?)data.Location ?? DBNull.Value;
            cmd.Parameters.Add(locationParam);

            // Execute and read JSON result
            var jsonResult = string.Empty;
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    jsonResult = reader.GetString(0);
                }
            }

            if (!string.IsNullOrWhiteSpace(jsonResult))
            {
                result = JsonConvert.DeserializeObject<List<BusinessSearchResultDto>>(jsonResult);
            }

            return result;
        }
    }
}
