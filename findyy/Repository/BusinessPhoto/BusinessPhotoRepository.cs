using findyy.Model.BusinessRegister;
using findyy.Repositories.BusinessPhotoRepository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class BusinessPhotoRepository : IBusinessPhotoRepository
{
    private readonly AppDbContext _db;

    public BusinessPhotoRepository(AppDbContext db)
    {
        _db = db;
    }

    // ---------- INSERT (POST) ----------
    public async Task AddAsync(BusinessPhoto photo)
    {
        var parameters = new[]
        {
            new SqlParameter("@Action", "POST"),
            new SqlParameter("@BusinessId", photo.BusinessId),
            new SqlParameter("@Url", photo.Url),
            new SqlParameter("@Caption", (object?)photo.Caption ?? DBNull.Value),
            new SqlParameter("@IsMain", SqlDbType.Bit) { Value = photo.IsMain }
        };

        await _db.Database.ExecuteSqlRawAsync(
            "EXEC sp_BusinessPhotos @Action=@Action, @BusinessId=@BusinessId, @Url=@Url, " +
            "@Caption=@Caption, @IsMain=@IsMain",
            parameters
        );
    }

    // ---------- SELECT BY BUSINESS ----------
    public async Task<IEnumerable<BusinessPhoto>> GetByBusinessIdAsync(long businessId)
    {
        var photos = await _db.BusinessPhoto
            .FromSqlInterpolated(
                $"EXEC sp_BusinessPhotos @Action = {"GET_BY_BUSINESS"}, @BusinessId = {businessId}")
            .ToListAsync();

        return photos;
    }

    // ---------- DELETE ALL BY BUSINESS ----------  👈 NEW
    public async Task DeleteByBusinessIdAsync(long businessId)
    {
        var parameters = new[]
        {
            new SqlParameter("@Action", "DELETE_BY_BUSINESS"),
            new SqlParameter("@BusinessId", businessId)
        };

        await _db.Database.ExecuteSqlRawAsync(
            "EXEC sp_BusinessPhotos @Action=@Action, @BusinessId=@BusinessId",
            parameters
        );
    }

    public async Task<BusinessPhoto?> GetByIdAsync(long id)
    {
        var list = await _db.BusinessPhoto
            .FromSqlInterpolated($"EXEC sp_BusinessPhotos @Action = {"GET"}, @Id = {id}")
            .ToListAsync();

        return list.FirstOrDefault();
    }

    public async Task DeleteByIdAsync(long id)
    {
        var parameters = new[]
        {
        new SqlParameter("@Action", "DELETE"),
        new SqlParameter("@Id", id)
    };

        await _db.Database.ExecuteSqlRawAsync(
            "EXEC sp_BusinessPhotos @Action=@Action, @Id=@Id",
            parameters
        );
    }

    // not using SP, just direct update – simple and fine
    public async Task ClearMainAsync(long businessId)
    {
        var param = new SqlParameter("@BusinessId", businessId);
        await _db.Database.ExecuteSqlRawAsync(
            "UPDATE BusinessPhoto SET IsMain = 0 WHERE BusinessId = @BusinessId",
            param
        );
    }

}
