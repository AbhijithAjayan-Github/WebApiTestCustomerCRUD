using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string?  CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
    }
}
