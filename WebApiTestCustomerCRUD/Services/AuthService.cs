using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiTestCustomerCRUD.Data;
using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;
using WebApiTestCustomerCRUD.Services.Interfaces;

namespace WebApiTestCustomerCRUD.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AuthService> logger;
        private readonly IConfiguration configuration;
        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger, IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            this.configuration = configuration;
        }
        public string GenerateAccessToken(User userDetails)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userDetails.Name),
                new Claim("userId",userDetails.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpiryInMinutes"])),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken(User userDetails)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userDetails.Name),
                new Claim("userId",userDetails.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["RefreshTokenExpiryInMinutes"])),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<UserLoginResponse> GenerateAccessTokenFromRefreshToken(string refreshToken)
        {
            UserLoginResponse response = new UserLoginResponse();
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = configuration.GetSection("JWT");
            var principal = handler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            }, out SecurityToken validatedToken);
            var userId = principal.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.Success = false;
                response.Message = "Empty/Null userId claim from refresh token";
                logger.LogInformation($"Empty/Null userId claim from refresh token");
            }
            else
            {
                var user = await context.Users.FindAsync(int.Parse(userId));
                if (user == null)
                {
                    response.Success = false;
                    response.UserId = int.Parse(userId);
                    response.Message = $"User with id {userId} not found";
                    logger.LogInformation($"User with id {userId} not found");
                }
                else
                {
                    response.UserId = user.UserId;
                    response.AccessToken = GenerateAccessToken(user);
                    response.RefreshToken = GenerateRefreshToken(user);
                    response.Success = true;
                    response.Message = $"Refresh token validated, Created new Access token and Refresh Token for user {user.UserId}";
                    logger.LogInformation($"Refresh token validated, Created new Access token and Refresh Token for user {user.UserId}");
                }
            }
            return response;
        }
        public async Task<UserRegisterResponse> RegisterUser(UserRegister userDetails)
        {
            UserRegisterResponse response = new UserRegisterResponse();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDetails.Password.Trim());
            User user = new User
            {
                Name = userDetails.UserName.Trim(),
                Email = userDetails.Email.Trim(),
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            logger.LogInformation($"Successfully Inserted User : {user.Name} into the Users Table");
            response.AccessToken = GenerateAccessToken(user);
            response.RefreshToken = GenerateRefreshToken(user);
            response.Success = true;
            response.Message = $"Successfully registered user : {userDetails.UserName}";
            logger.LogInformation($"Successfully registered user : {userDetails.UserName}");
            return response;
        }
        public async Task<UserLoginResponse> UserLogin(UserLogin user)
        {
            UserLoginResponse response = new UserLoginResponse();
            var loginUser = await context.Users.FirstOrDefaultAsync(u => u.Name == user.UserName);
            if (loginUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, loginUser.Password))
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                logger.LogInformation($"Login user: {user.UserName} failed");
            }
            else
            {
                response.Success = true;
                response.Message = "Login successfull";
                response.UserId = loginUser.UserId;
                response.AccessToken = GenerateAccessToken(loginUser);
                response.RefreshToken = GenerateRefreshToken(loginUser);
                logger.LogInformation($"Login user: {user.UserName} successfull");
            }
            return response;
        }
    }
}
