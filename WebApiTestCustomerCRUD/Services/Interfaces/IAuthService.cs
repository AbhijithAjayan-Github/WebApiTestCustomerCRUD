using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateAccessToken(User userDetails);
        string GenerateRefreshToken(User userDetails);
        Task<UserLoginResponse> GenerateAccessTokenFromRefreshToken(string refreshToken);
        Task<UserRegisterResponse> RegisterUser(UserRegister userDetails);
        Task<UserLoginResponse> UserLogin(UserLogin user);

    }
}
