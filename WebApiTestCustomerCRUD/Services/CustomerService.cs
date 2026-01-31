using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApiTestCustomerCRUD.Data;
using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;
using WebApiTestCustomerCRUD.Services.Helpers;
using WebApiTestCustomerCRUD.Services.Interfaces;

namespace WebApiTestCustomerCRUD.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ApplicationDbContext context;
        private readonly TimeZoneHelperService timeZoneHelper;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(ApplicationDbContext context,ILogger<CustomerService> logger,TimeZoneHelperService timeZoneHelper)
        {
            this.context = context;
            this.logger = logger;
            this.timeZoneHelper = timeZoneHelper;
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
                customer.CreatedAt = timeZoneHelper.ConvertToIST(customer.CreatedAt); // we are converting time to corresponding timezone we set in appsettings.json to the client
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
        
        public async Task<CustomerAddResponse> UpdateCustomerById(CustomerUpdate customerDetails)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            try
            {
                var customer = await context.Customers.FirstOrDefaultAsync(cus => cus.CustomerId == customerDetails.CustomerId);
                if (customer == null)
                {
                    response.Sucess = false;
                    response.Message = $"Failed to fetch customer with id {customerDetails.CustomerId}";
                    logger.LogInformation($"Failed to fetch customer with id {customerDetails.CustomerId}");
                    return response;
                }
                customer.CustomerName = customerDetails.CustomerName;
                customer.CustomerAddress = customerDetails.CustomerAddress;
                customer.CustomerEmail = customerDetails.CustomerEmail;
                customer.CustomerPhone = customerDetails.CustomerPhone;
                customer.UpdatedBy = customerDetails.UpdatedBy;
                customer.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                response.Sucess = true;
                response.Message = $"Successfully updated customer info of customer {customer.CustomerName}, customer Id : {customer.CustomerId}";
            }
            catch(Exception ex)
            {
                response.Sucess= false;
                response.Message = $"Error : {ex.Message}";
                logger.LogError($"Error : {ex.Message}");
            }
            return response;
        }

        public async Task<CustomerAddResponse> DeleteCustomerById(int customerId)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            try
            {
                var customer = context.Customers.FirstOrDefault(x=>x.CustomerId == customerId);
                if(customer == null)
                {
                    response.Sucess = false;
                    response.Message = $"Customer with id {customerId} not found";
                    logger.LogInformation($"Customer with id {customerId} not found");
                    return response;
                }
                context.Customers.Remove(customer);
                await context.SaveChangesAsync();
                response.Sucess = true;
                response.Message = $"Successfully deleted customer :{customerId}, customer name : {customer.CustomerName}";
            }
            catch(Exception ex)
            {
                response.Sucess = false;
                response.Message = $"Error : {ex.Message}";
            }
            return response;
        }
    }
}
