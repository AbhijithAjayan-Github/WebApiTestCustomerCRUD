using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.DTOs.Responses
{
    public class GetCustomerByIdResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        
        public Customer customer { get; set; }
    }
}
