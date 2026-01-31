using System.Text.Json;
using WebApiTestCustomerCRUD.Data;
using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;
using WebApiTestCustomerCRUD.Services.Interfaces;

namespace WebApiTestCustomerCRUD.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(ApplicationDbContext context,ILogger<CustomerService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task<CustomerAddResponse>CustomerAdd(CustomerAdd customerDTO)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            try
            {
                Customer customer = new Customer
                {
                    CustomerName = customerDTO.Name.Trim(),
                    CustomerAddress = customerDTO.Address.Trim(),
                    CustomerEmail = customerDTO.Email.Trim(),
                    CustomerPhone = customerDTO.Phone.Trim(),
                    CreatedBy = customerDTO.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                response.Sucess = true;
                response.Message = $"Successfully added user {customer.CustomerName}";
                logger.LogInformation($"{JsonSerializer.Serialize(response)}");
            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Message = $"Error : {ex.Message}";
            }
            return response;
        }
    }
}
