using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.DTOs.Requests
{
    public class CustomerUpdate
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        [Required]
        public string CustomerPhone { get; set; }
        [Required]
        public int UpdatedBy { get; set; }
    }
}
