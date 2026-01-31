using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;

namespace WebApiTestCustomerCRUD.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerAddResponse> CustomerAdd(CustomerAdd customer);
        Task<GetCustomerByIdResponse> GetCustomerById(int customerId);
        Task<CustomerAddResponse> UpdateCustomerById(CustomerUpdate customerDetails);
        Task<CustomerAddResponse> DeleteCustomerById(int customerId);
        Task<GetAllCustomersResponse> GetAllCustomers();
        Task<PaginatedCustomersResponse> GetPaginatedCustomers(GetPaginatedCustomers getCustomers);
    }
}
