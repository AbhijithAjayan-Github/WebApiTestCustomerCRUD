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
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext context;
        private readonly TimeZoneHelperService timeZoneHelper;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(ApplicationDbContext context, ILogger<CustomerService> logger, TimeZoneHelperService timeZoneHelper)
        {
            this.context = context;
            this.logger = logger;
            this.timeZoneHelper = timeZoneHelper;
        }
        public async Task<CustomerAddResponse> CustomerAdd(CustomerAdd customerDTO)
        {
            CustomerAddResponse response = new CustomerAddResponse();
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
            return response;
        }
        public async Task<GetCustomerByIdResponse> GetCustomerById(int customerId)
        {
            GetCustomerByIdResponse response = new GetCustomerByIdResponse();
            var customer = await context.Customers.FirstOrDefaultAsync(cus => cus.CustomerId == customerId);
            logger.LogInformation($"Customer fetched for id {customerId}: {JsonSerializer.Serialize(customer)}");
            if (customer == null) throw new KeyNotFoundException($"Customer with Id {customerId} not found");
            customer.CreatedAt = timeZoneHelper.ConvertToIST(customer.CreatedAt); // we are converting time to corresponding timezone we set in appsettings.json to the client
            response.Success = true;
            response.Message = $"Successfully fetched customer for {customerId}";
            response.customer = customer;
            return response;
        }

        public async Task<CustomerAddResponse> UpdateCustomerById(CustomerUpdate customerDetails)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            var customer = await context.Customers.FirstOrDefaultAsync(cus => cus.CustomerId == customerDetails.CustomerId);
            if (customer == null) throw new KeyNotFoundException($"Customer with Id {customerDetails.CustomerId} not found");
            customer.CustomerName = customerDetails.CustomerName;
            customer.CustomerAddress = customerDetails.CustomerAddress;
            customer.CustomerEmail = customerDetails.CustomerEmail;
            customer.CustomerPhone = customerDetails.CustomerPhone;
            customer.UpdatedBy = customerDetails.UpdatedBy;
            customer.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            response.Sucess = true;
            response.Message = $"Successfully updated customer info of customer {customer.CustomerName}, customer Id : {customer.CustomerId}";
            return response;
        }

        public async Task<CustomerAddResponse> DeleteCustomerById(int customerId)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            var customer = context.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) throw new KeyNotFoundException($"Customer with Id {customerId} not found");
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            response.Sucess = true;
            response.Message = $"Successfully deleted customer :{customerId}, customer name : {customer.CustomerName}";
            return response;
        }

        public async Task<GetAllCustomersResponse> GetAllCustomers()
        {
            GetAllCustomersResponse response = new GetAllCustomersResponse();
            var customers = await context.Customers.Select(cus => new Customer
            {
                CustomerId = cus.CustomerId,
                CustomerName = cus.CustomerName,
                CustomerEmail = cus.CustomerEmail,
                CustomerPhone = cus.CustomerPhone,
                CustomerAddress = cus.CustomerAddress,
                CreatedBy = cus.CreatedBy,
                CreatedAt = timeZoneHelper.ConvertToIST(cus.CreatedAt),
                UpdatedBy = cus.UpdatedBy,
                UpdatedAt = timeZoneHelper.ConvertToIST(cus.UpdatedAt)
            }).ToListAsync();
            if (customers == null) throw new KeyNotFoundException("No customer details available");
            response.Success = true;
            response.message = $"Successfully fetched customers";
            response.TotalCustomerCount = customers.Count;
            response.customers = customers;
            return response;
        }
        public async Task<PaginatedCustomersResponse> GetPaginatedCustomers(GetPaginatedCustomers getCustomers)
        {
            PaginatedCustomersResponse response = new PaginatedCustomersResponse();
            int skipCount = (getCustomers.CurrentPage - 1) * getCustomers.PageSize;
            var customersCount = await context.Customers.CountAsync();
            if (customersCount == 0)
            {
                response.Success = false;
                response.Message = $"No customers found";
                response.PageSize = getCustomers.PageSize;
                response.CurrentPage = getCustomers.CurrentPage;
                response.TotalCustomerCount = customersCount;
                response.Customers = new List<Customer>();
                logger.LogInformation($"{JsonSerializer.Serialize(response)}");
                return response;
            }
            int totalPages = (int)Math.Ceiling((double)customersCount / getCustomers.PageSize);
            var customers = await context.Customers.Skip(skipCount).Take(getCustomers.PageSize).Select(cus => new Customer
            {
                CustomerId = cus.CustomerId,
                CustomerName = cus.CustomerName,
                CustomerEmail = cus.CustomerEmail,
                CustomerPhone = cus.CustomerPhone,
                CustomerAddress = cus.CustomerAddress,
                CreatedBy = cus.CreatedBy,
                CreatedAt = timeZoneHelper.ConvertToIST(cus.CreatedAt),
                UpdatedBy = cus.UpdatedBy,
                UpdatedAt = timeZoneHelper.ConvertToIST(cus.UpdatedAt)
            }).ToListAsync();
            response.Success = true;
            response.Message = "Successfully fetched customers";
            response.PageSize = getCustomers.PageSize;
            response.CurrentPage = getCustomers.CurrentPage;
            response.TotalPages = totalPages;
            response.TotalCustomerCount = customersCount;
            response.Customers = customers;
            return response;
        }
    }
}

