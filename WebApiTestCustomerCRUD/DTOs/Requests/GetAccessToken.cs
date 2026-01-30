using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.DTOs.Requests
{
    public class GetAccessToken
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
