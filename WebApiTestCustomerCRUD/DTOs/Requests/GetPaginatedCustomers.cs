using System.ComponentModel.DataAnnotations;

namespace WebApiTestCustomerCRUD.DTOs.Requests
{
    public class GetPaginatedCustomers
    {
        [Required]
        public int CurrentPage { get; set; } = 1;
        [Required]
        public int PageSize { get; set; } = 1;
    }
}
