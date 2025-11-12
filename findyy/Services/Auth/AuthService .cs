using findyy.DTO.Auth;
using findyy.Model.BusinessRegister;
using findyy.Model.Response;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Services.Auth.Interface;
using LocalBizFinder.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LocalBizFinder.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IBusinessRepository _businessrepo;
        private readonly IConfiguration _Configuration;

        public IPasswordHasher<UserDto> _passwordHasher { get; }
        public IEmailService _emailService { get; }

        public AuthService(IUserRepository repo, IBusinessRepository businessrepo, IPasswordHasher<UserDto> passwordHasher, IConfiguration configuration,
            IEmailService emailService)
        {
            _repo = repo;
            _businessrepo = businessrepo;
            _passwordHasher = passwordHasher;
            _Configuration = configuration;
            _emailService = emailService;
        }

        public async Task<Response> RegisterAsync(UserDto dto)
        {
            var userExists = await _repo.GetUserAsync(dto.Email);

            if (userExists != null)
            {
                return new Response
                {
                    Status = false,
                    Message = "User Already Exists!",
                    Data = null
                };
            }

            var hashPassword = _passwordHasher.HashPassword(dto, dto.Password);

            // ✅ Generate token BEFORE saving user
            var verificationToken = Guid.NewGuid().ToString();

            var userInfo = new RegisterDto()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BusinessCategoryId = dto.BusinessCategoryId,
                BusinessName = dto.BusinessName,
                Email = dto.Email,
                Password = hashPassword,
                Role = dto.Role,
                EmailVerificationToken = verificationToken,
                VerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            // ✅ Only save ONCE
            await _repo.RegisterAsync(userInfo);

            var verificationLink = $"{_Configuration["AppUrl"]}/verifyemail?token={verificationToken}&email={userInfo.Email}";
            await _emailService.SendEmailAsync(dto.Email, "Verify your email",
                $"Hi {dto.FirstName} {dto.LastName},<br/><br/>Please verify your account by clicking <a href='{verificationLink}'>here</a>.<br/><br/>Thanks!");

            return new Response
            {
                Status = true,
                Message = "User registered successfully. Please check your email for verification.",
                Data = null
            };
        }

        public async Task<Response?> LoginAsync(LoginDto dto)
        {
            var userExists = await _repo.GetUserAsync(dto.Email);
            if (userExists == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "User Not Exists",
                    Data = null
                };
            }
            var userInfo = new UserDto()
            {
                Email = userExists.Email,
                Password = userExists.PasswordHash,
                FirstName = userExists.FirstName,
                LastName = userExists.LastName,
                Role = userExists.Role
            };
            var matchPassword = _passwordHasher.VerifyHashedPassword(userInfo, userExists.PasswordHash, dto.Password);
            if (matchPassword == PasswordVerificationResult.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]);

                long? businessId = null;

                if (userExists.Role.Equals("Business", StringComparison.OrdinalIgnoreCase))
                {
                    var business = await _businessrepo.GetBusinessAsync(userExists.Id);

                    if (business != null)
                    {
                        businessId = business.Id;
                    }
                }


                var claims = new List<Claim>
                {
                    new Claim("role", userExists.Role),
                        new Claim("email", userExists.Email),
                        new Claim("businessName", userExists.BusinessName),
                        new Claim("businessCategory", userExists.BusinessCategoryId.ToString()),
                        new Claim("first_name", userExists.FirstName),
                        new Claim("last_name", userExists.LastName),
                        new Claim("UserId", userExists.Id.ToString(),  ClaimValueTypes.Integer),
                };

                if (businessId.HasValue)
                {
                    claims.Add(new Claim("BusinessId", businessId.Value.ToString(), ClaimValueTypes.Integer));
                }


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = _Configuration["Jwt:Issuer"],
                    Audience = _Configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new Response
                {
                    Status = true,
                    Message = "Login Successfully!",
                    Data = tokenString
                };

            }
            else
            {
                return new Response
                {
                    Status = false,
                    Message = "Wrong Password!",
                    Data = null
                };
            }
        }

        public async Task<Response> VerifyEmailAsync(string token)
        {
            var user = await _repo.GetUserByTokenAsync(token);
            if (user == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Invalid or expired token",
                    Data = null
                };
            }

            await _repo.VerifyUserAsync(token);

            return new Response
            {
                Status = true,
                Message = "Email verified successfully!",
                Data = null
            };
        }


    }
}
