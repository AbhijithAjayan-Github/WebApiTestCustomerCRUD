using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.DTOs.Requests
{
    public class CustomerAdd
    {
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
}
