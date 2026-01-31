using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;

namespace WebApiTestCustomerCRUD.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerAddResponse> CustomerAdd(CustomerAdd customer);
    }
}
