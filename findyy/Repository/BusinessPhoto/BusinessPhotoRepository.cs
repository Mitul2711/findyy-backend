using findyy.Model.BusinessRegister;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using findyy.Repositories.BusinessPhotoRepository.Interface;

namespace findyy.Repositories.BusinessPhotoRepository
{
    public class BusinessPhotoRepository : IBusinessPhotoRepository
    {
        private readonly DbContext _context; // inject your actual DbContext (e.g., AppDbContext)

        public BusinessPhotoRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<long> AddAsync(BusinessPhoto photo)
        {
            using var conn = _context.Database.GetDbConnection();
            await EnsureOpenAsync(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_BusinessPhotos";

            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = "Add" });
            cmd.Parameters.Add(new SqlParameter("@BusinessId", SqlDbType.BigInt) { Value = photo.BusinessId });
            cmd.Parameters.Add(new SqlParameter("@Url", SqlDbType.NVarChar, 1000) { Value = photo.Url });
            cmd.Parameters.Add(new SqlParameter("@Caption", SqlDbType.NVarChar, 250) { Value = (object?)photo.Caption ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@IsMain", SqlDbType.Bit) { Value = photo.IsMain });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var id = Convert.ToInt64(reader["InsertedId"]);
                return id;
            }
            throw new Exception("Failed to insert business photo.");
        }

        public async Task<bool> SetMainAsync(long businessId, long photoId)
        {
            using var conn = _context.Database.GetDbConnection();
            await EnsureOpenAsync(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_BusinessPhotos";

            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = "SetMain" });
            cmd.Parameters.Add(new SqlParameter("@BusinessId", SqlDbType.BigInt) { Value = businessId });
            cmd.Parameters.Add(new SqlParameter("@PhotoId", SqlDbType.BigInt) { Value = photoId });

            using var reader = await cmd.ExecuteReaderAsync();
            // we returned a "1 AS Success" row in SP
            if (await reader.ReadAsync()) return true;
            return false;
        }

        public async Task<string?> DeleteAsync(long businessId, long photoId)
        {
            using var conn = _context.Database.GetDbConnection();
            await EnsureOpenAsync(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_BusinessPhotos";

            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = "Delete" });
            cmd.Parameters.Add(new SqlParameter("@BusinessId", SqlDbType.BigInt) { Value = businessId });
            cmd.Parameters.Add(new SqlParameter("@PhotoId", SqlDbType.BigInt) { Value = photoId });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var url = reader["Url"] as string;
                // reader will be before delete row select; SP then deletes row. Return url to service so it can delete file.
                return url;
            }
            return null;
        }

        public async Task<IList<BusinessPhoto>> GetByBusinessAsync(long businessId)
        {
            var list = new List<BusinessPhoto>();
            using var conn = _context.Database.GetDbConnection();
            await EnsureOpenAsync(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_BusinessPhotos";

            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = "GetByBusiness" });
            cmd.Parameters.Add(new SqlParameter("@BusinessId", SqlDbType.BigInt) { Value = businessId });

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<BusinessPhoto?> GetMainAsync(long businessId)
        {
            using var conn = _context.Database.GetDbConnection();
            await EnsureOpenAsync(conn);

            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.sp_BusinessPhotos";

            cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = "GetMain" });
            cmd.Parameters.Add(new SqlParameter("@BusinessId", SqlDbType.BigInt) { Value = businessId });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        private BusinessPhoto MapReader(System.Data.Common.DbDataReader reader)
        {
            return new BusinessPhoto
            {
                Id = Convert.ToInt64(reader["Id"]),
                BusinessId = Convert.ToInt64(reader["BusinessId"]),
                Url = reader["Url"] as string ?? string.Empty,
                Caption = reader["Caption"] as string,
                IsMain = Convert.ToBoolean(reader["IsMain"]),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["UpdatedAt"])
            };
        }

        private async Task EnsureOpenAsync(System.Data.Common.DbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
        }
    }
}
