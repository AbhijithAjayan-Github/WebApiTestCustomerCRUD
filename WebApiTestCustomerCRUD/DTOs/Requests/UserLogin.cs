using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.DTOs.Requests
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
