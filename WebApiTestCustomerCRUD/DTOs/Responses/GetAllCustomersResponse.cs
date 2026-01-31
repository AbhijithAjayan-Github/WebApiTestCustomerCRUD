using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.DTOs.Responses
{
    public class GetAllCustomersResponse
    {
        public bool Success { get; set; }
        public string message { get; set; }
        public int TotalCustomerCount { get; set; }
        public List<Customer> customers { get; set; } = new List<Customer>();
    }
}
