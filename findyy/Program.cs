using findyy.DTO.Auth;
using findyy.Repository.BusinessCategoryRepo.Interface;
using findyy.Repository.BusinessDash;
using findyy.Repository.BusinessDash.Interface;
using findyy.Repository.BusinessRegister;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Services.Admin;
using findyy.Services.Admin.Interface;
using findyy.Services.Auth;
using findyy.Services.Auth.Interface;
using findyy.Services.BusinessDash;
using findyy.Services.BusinessDash.Interface;
using findyy.Services.BusinessRegister;
using findyy.Services.BusinessRegister.Interface;
using LocalBizFinder.Business.Services;
using LocalBizFinder.DataAccess.Interfaces;
using LocalBizFinder.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

// Repository & Services
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

builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();

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
});

// ===== Authorization =====
builder.Services.AddAuthorization();

// ===== CORS for Angular frontend =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins(builder.Configuration["AppUrl"])
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// ===== Middleware pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// order matters: Auth before Controllers
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
