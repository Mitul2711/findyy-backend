using findyy.DTO.Auth;
using findyy.Repository.BusinessCategoryRepo.Interface;
using findyy.Repository.BusinessDash;
using findyy.Repository.BusinessDash.Interface;
using findyy.Repository.BusinessRegister;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Repository.BusinessReviewRepo;
using findyy.Repository.BusinessReviewRepo.Interface;
using findyy.Repository.ChatMessageRepo;
using findyy.Repository.ChatMessageRepo.Interface;
using findyy.Repository.SearchBusiness;
using findyy.Repository.SearchBusiness.Interface;
using findyy.Services.Admin;
using findyy.Services.Admin.Interface;
using findyy.Services.Auth;
using findyy.Services.Auth.Interface;
using findyy.Services.BusinessDash;
using findyy.Services.BusinessDash.Interface;
using findyy.Services.BusinessRegister;
using findyy.Services.BusinessRegister.Interface;
using findyy.Services.BusinessReview;
using findyy.Services.BusinessReview.Interface;
using findyy.Services.ChatMessage;
using findyy.Services.ChatMessage.Interface;
using findyy.Services.ChatMessageService;
using findyy.Services.SearchBusiness;
using findyy.Services.SearchBusiness.Interface;
using LocalBizFinder.Business.Services;
using LocalBizFinder.DataAccess.Interfaces;
using LocalBizFinder.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== Controllers & JSON Options =====
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== Database Context =====
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

// ===== Repositories & Services =====
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();

builder.Services.AddScoped<IBusinessDashRepository, BusinessDashRepository>();
builder.Services.AddScoped<IBusinessDashService, BusinessDashService>();
builder.Services.AddScoped<IPasswordHasher<UserDto>, PasswordHasher<UserDto>>();

builder.Services.AddScoped<IBusinessCategoryRepository, BusinessCategoryRepository>();
builder.Services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();

builder.Services.AddScoped<ISearchBusinessRepository, SearchBusinessRepository>();
builder.Services.AddScoped<ISearchBusinessService, SearchBusinessService>();

builder.Services.AddScoped<IBusinessReviewRepository, BusinessReviewRepository>();
builder.Services.AddScoped<IBusinessReviewService, BusinessReviewService>();

builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();

// ===== Add SignalR =====
builder.Services.AddSignalR();

// ===== JWT Authentication =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // ✅ Allow JWT token in query string for SignalR connections
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== CORS for Angular frontend =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins(builder.Configuration["AppUrl"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // ✅ Required for SignalR
});

var app = builder.Build();

// ===== Middleware pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// ===== Map Controllers =====
app.MapControllers();

// ===== Map SignalR Hub =====
app.MapHub<ChatHub>("/chatHub"); // ✅ ChatHub endpoint

app.Run();
