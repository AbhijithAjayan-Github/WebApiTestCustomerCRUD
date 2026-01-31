using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.DTOs.Responses
{
    public class PaginatedCustomersResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; }
        public int TotalPages { get; set; } = 1;
        public int TotalCustomerCount { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
