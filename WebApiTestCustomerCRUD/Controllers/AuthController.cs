    using Microsoft.AspNetCore.Mvc;
    using WebApiTestCustomerCRUD.DTOs.Requests;
    using WebApiTestCustomerCRUD.DTOs.Responses;
    using WebApiTestCustomerCRUD.Services;

    namespace WebApiTestCustomerCRUD.Controllers
    {
        [Route("api/auth")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly ILogger<AuthController> logger;
            private readonly AuthService authService;
            public AuthController(ILogger<AuthController>logger,AuthService authService)
            {
                this.authService = authService;
                this.logger = logger;
            }
            [HttpPost]
            [Route("register")]
            public async Task<IActionResult>Register(UserRegister userDetails)
            {
                UserRegisterResponse response = new UserRegisterResponse();
                try
                {
                    response = await authService.RegisterUser(userDetails);
                }
                catch(Exception ex)
                {
                    logger.LogInformation($"Exception : {ex.Message}");
                }
                return Ok(response);
            }
            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login(UserLogin userDetails)
            {
                UserLoginResponse response = new UserLoginResponse();
                try
                {
                    response = await authService.UserLogin(userDetails);
                }
                catch (Exception ex)                
                {
                    logger.LogInformation($"Exception : {ex.Message}");
                }
                return Ok(response);
            }
            [HttpPost]
            [Route("get_access_token")]
            public async Task<IActionResult>GetAccessToken(GetAccessToken refreshToken)
            {   
                UserLoginResponse response = new UserLoginResponse();
            try
            {
                response = await authService.GenerateAccessTokenFromRefreshToken(refreshToken.RefreshToken);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error : {ex.Message}");
            }
                return Ok(response);
            }
        }
    }
