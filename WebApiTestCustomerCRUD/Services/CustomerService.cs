using Microsoft.EntityFrameworkCore;
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
        public async Task<GetCustomerByIdResponse> GetCustomerById(int customerId)
        {
            GetCustomerByIdResponse response = new GetCustomerByIdResponse();
            try
            {
                var customer = await context.Customers.FirstOrDefaultAsync(cus => cus.CustomerId == customerId);
                logger.LogInformation($"Customer fetched for id {customerId}: {JsonSerializer.Serialize(customer)}");
                if (customer == null)
                {
                    response.Success = false;
                    response.Message = $"Customer with id {customerId} not found";
                    response.customer = new Customer();
                    return response;
                }
                response.Success = true;
                response.Message = $"Successfully fetched customer for {customerId}";
                response.customer = customer;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error : {ex.Message}";
                response.customer = new Customer();
            }
            return response;
        }
    }
}
