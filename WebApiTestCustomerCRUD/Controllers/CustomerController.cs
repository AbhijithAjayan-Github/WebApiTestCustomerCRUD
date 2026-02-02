using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
using WebApiTestCustomerCRUD.Models;
using WebApiTestCustomerCRUD.Services.Interfaces;

namespace WebApiTestCustomerCRUD.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerController> logger;
        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            this.logger = logger;
            this.customerService = customerService;
        }

        [HttpPost]
        [Authorize]
        [Route("customer_add")]
        public async Task<IActionResult> CustomerAdd(CustomerAdd customerDto)
        {
            CustomerAddResponse response = new CustomerAddResponse();

            if (customerDto.CreatedBy == 0)
            {
                response.Sucess = false;
                response.Message = $"Invalid UserId 0";
                logger.LogInformation($"{JsonSerializer.Serialize(response)}");
                return BadRequest(response);
            }
            response = await customerService.CustomerAdd(customerDto);

            return Ok(response);
        }

        [HttpGet("get_customer/{customerId}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            GetCustomerByIdResponse response = new GetCustomerByIdResponse();
            if (customerId == 0)
            {
                response.Success = false;
                response.Message = $"Invalid customer id 0";
                response.customer = new Customer();
                return BadRequest(response);
            }
            response = await customerService.GetCustomerById(customerId);
            return Ok(response);
        }
        [HttpPut]
        [Route("update_customer")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerById(CustomerUpdate customerDTO)
        {
            CustomerAddResponse response = new CustomerAddResponse();

            if (customerDTO.CustomerId == 0 || customerDTO.UpdatedBy == 0)
            {
                response.Sucess = false;
                response.Message = $"Invalid Customer Id / User Id -> Customer Id : {customerDTO.CustomerId}, User Id : {customerDTO.UpdatedBy}";
                logger.LogInformation($"Invalid Customer Id / User Id -> Customer Id : {customerDTO.CustomerId}, User Id : {customerDTO.UpdatedBy}");
                return BadRequest(response);
            }
            response = await customerService.UpdateCustomerById(customerDTO);
            return Ok(response);
        }

        [HttpDelete("delete_customer/{customerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomerById(int customerId)
        {
            CustomerAddResponse response = new CustomerAddResponse();

            if (customerId == 0)
            {
                response.Sucess = false;
                response.Message = $"Invalid customer id : {customerId}";
            }
            response = await customerService.DeleteCustomerById(customerId);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [Route("get_all_customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            GetAllCustomersResponse response = new GetAllCustomersResponse();
            response = await customerService.GetAllCustomers();
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        [Route("get_customers")]
        public async Task<IActionResult> GetPaginatedCustomers(GetPaginatedCustomers getData)
        {
            PaginatedCustomersResponse response = new PaginatedCustomersResponse();
            response = await customerService.GetPaginatedCustomers(getData);
            return Ok(response);
        }
    }
}
