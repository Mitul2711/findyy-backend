using findyy.DTO;
using findyy.DTO.ChatMessageDTO;
using findyy.Model;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Repository.ChatMessageRepo.Interface;
using LocalBizFinder.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace findyy.Repository.ChatMessageRepo
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly AppDbContext _db;
        private readonly IBusinessRepository _businessRepo;
        private readonly IUserRepository _userRepo;
        public ChatMessageRepository(AppDbContext db, IBusinessRepository businessRepo, IUserRepository userRepo)
        {
            _db = db;
            _businessRepo = businessRepo;
            _userRepo = userRepo;
        }

        // ✅ 1️⃣ Get full chat between two users
        public async Task<ChatMessageResponseDTO> GetChatBetweenUsersAsync(Guid senderId, Guid receiverId)
        {
            var chatResponse = new ChatMessageResponseDTO();

            await using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_GetChatBetweenUsers";
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameters
            cmd.Parameters.Add(CreateParam(cmd, "@SenderId", senderId));
            cmd.Parameters.Add(CreateParam(cmd, "@ReceiverId", receiverId));
            cmd.Parameters.Add(CreateParam(cmd, "@Action", "GetChatBetweenUsers"));

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var lastMessageJson = reader["LastMessage"]?.ToString();
                var chatHistoryJson = reader["ChatHistory"]?.ToString();

                chatResponse.LastMessage = !string.IsNullOrEmpty(lastMessageJson)
                    ? JsonConvert.DeserializeObject<ChatMessageModel>(lastMessageJson)
                    : null;

                chatResponse.ChatHistory = !string.IsNullOrEmpty(chatHistoryJson)
                    ? JsonConvert.DeserializeObject<List<ChatMessageModel>>(chatHistoryJson)
                    : new List<ChatMessageModel>();
            }

            return chatResponse;
        }

        public async Task<List<ChatListDTO>> GetAllChatsForUserAsync(Guid senderId)
        {
            var chatList = new List<ChatListDTO>();

            await using var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_GetChatBetweenUsers";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(CreateParam(cmd, "@SenderId", senderId));
            cmd.Parameters.Add(CreateParam(cmd, "@ReceiverId", DBNull.Value));
            cmd.Parameters.Add(CreateParam(cmd, "@Action", "GetAllChatsForUser"));

            string jsonResult = string.Empty;

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    jsonResult = reader.GetString(0);
                }
            }

            if (!string.IsNullOrWhiteSpace(jsonResult))
            {
                var tempList = Newtonsoft.Json.Linq.JArray.Parse(jsonResult);

                foreach (var item in tempList)
                {
                    var chatDto = new ChatListDTO
                    {
                        ReceiverId = item["ReceiverId"]?.ToObject<Guid>() ?? Guid.Empty,
                        LastMessage = ParseLastMessage(item["LastMessage"]),
                    };

                    chatList.Add(chatDto);
                }
            }

            // ✅ Fetch business or user info
            foreach (var chat in chatList)
            {
                if (chat.ReceiverId == Guid.Empty)
                    continue;

                // Try getting business
                var business = await _businessRepo.GetBusinessAsync(chat.ReceiverId);

                if (business != null)
                {
                    chat.Business = new BusinessSearchResultDto
                    {
                        BusinessId = (int)business.Id,
                        OwnerUserId = business.OwnerUserId,
                        BusinessName = business.Name,
                        Description = business.Description,
                        Website = business.Website,
                        Phone = business.Phone,
                        Email = business.Email,
                        IsVerified = business.IsVerified,
                        Status = business.Status switch
                        {
                            0 => "Pending",
                            1 => "Active",
                            2 => "Rejected",
                            3 => "Blocked",
                            _ => "Unknown"
                        },
                        AvgRating = business.AvgRating,
                        ReviewCount = business.ReviewCount,
                        CreatedAt = business.CreatedAt,
                        UpdatedAt = business.UpdatedAt,
                        CategoryName = business.BusinessCategory?.Name ?? string.Empty,

                        Location = business.Location == null ? null : new BusinessLocationDto
                        {
                            LocationId = (int)business.Location.Id,
                            AddressLine1 = business.Location.AddressLine1,
                            AddressLine2 = business.Location.AddressLine2,
                            City = business.Location.City,
                            State = business.Location.State,
                            Country = business.Location.Country,
                            PostalCode = business.Location.PostalCode ?? string.Empty,
                            Latitude = (decimal?)business.Location.Latitude,
                            Longitude = (decimal?)business.Location.Longitude
                        },

                        Hours = business.Hours?.Select(h => new BusinessHourDto
                        {
                            BusinessHourId = (int)h.Id,
                            DayOfWeek = Enum.GetName(typeof(DayOfWeek), h.DayOfWeek) ?? h.DayOfWeek.ToString(),
                            OpenTime = h.OpenTime,
                            CloseTime = h.CloseTime,
                            IsClosed = h.IsClosed
                        }).ToList() ?? new List<BusinessHourDto>()
                    };
                }
                else
                {
                    // ✅ If no business found → get user info
                    var user = await _userRepo.GetUserByIdAsync(chat.ReceiverId);

                    if (user != null)
                    {
                        chat.UserInfo = new UserInfoDto
                        {
                            Id = user.Id,
                            Name = user.FirstName + ' ' + user.LastName,
                            Email = user.Email,
                            Phone = user.Phone,
                            Role = user.Role,
                            City = user.City
                        };
                    }
                }
            }

            return chatList;
        }


        // Helper method to safely parse the "LastMessage" string or object
        private ChatMessageModel? ParseLastMessage(Newtonsoft.Json.Linq.JToken? token)
        {
            if (token == null) return null;

            // Case 1: It’s already an object
            if (token.Type == Newtonsoft.Json.Linq.JTokenType.Object)
            {
                return token.ToObject<ChatMessageModel>();
            }

            // Case 2: It’s a string containing JSON
            if (token.Type == Newtonsoft.Json.Linq.JTokenType.String)
            {
                var json = token.ToString();
                return JsonConvert.DeserializeObject<ChatMessageModel>(json);
            }

            return null;
        }

        // ✅ Helper method to reduce repetition
        private static IDbDataParameter CreateParam(IDbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            return param;
        }
    }
}
