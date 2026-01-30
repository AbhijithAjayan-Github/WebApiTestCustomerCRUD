namespace WebApiTestCustomerCRUD.DTOs.Responses
{
    public class UserRegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
