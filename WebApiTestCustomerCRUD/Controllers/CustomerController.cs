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
        public CustomerController(ILogger<CustomerController> logger,ICustomerService customerService)
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
            try
            {
                if(customerDto.CreatedBy == 0)
                {
                    response.Sucess = false;
                    response.Message = $"Invalid UserId 0";
                    logger.LogInformation($"{JsonSerializer.Serialize(response)}");
                    return BadRequest(response);
                }
                response = await customerService.CustomerAdd(customerDto);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error : {ex.Message}");
            }
            return Ok(response);
        }

        [HttpGet("get_customer/{customerId}")]
        [Authorize]
        public async Task<IActionResult>GetCustomerById(int customerId)
        {
            GetCustomerByIdResponse response = new GetCustomerByIdResponse();   
            try
            {
                if(customerId == 0)
                {
                    response.Success = false;
                    response.Message = $"Invalid customer id 0";
                    response.customer = new Customer();
                    return BadRequest(response);
                }
                response = await customerService.GetCustomerById(customerId);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Error : {ex.Message}");
            }
            return Ok(response);
        }
        [HttpPut]
        [Route("update_customer")]
        [Authorize]
        public async Task<IActionResult>UpdateCustomerById(CustomerUpdate customerDTO)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            try
            {
                if(customerDTO.CustomerId == 0 || customerDTO.UpdatedBy == 0)
                {
                    response.Sucess = false;
                    response.Message = $"Invalid Customer Id / User Id -> Customer Id : {customerDTO.CustomerId}, User Id : {customerDTO.UpdatedBy}";
                    logger.LogInformation($"Invalid Customer Id / User Id -> Customer Id : {customerDTO.CustomerId}, User Id : {customerDTO.UpdatedBy}");
                    return BadRequest(response);
                }
                response = await customerService.UpdateCustomerById(customerDTO);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"{ex.Message}", ex);
            }
            return Ok(response);
        }
        [HttpDelete("delete_customer/{customerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomerById(int customerId)
        {
            CustomerAddResponse response = new CustomerAddResponse();
            try
            {
               if(customerId == 0)
               {
                    response.Sucess = false;
                    response.Message = $"Invalid customer id : {customerId}";
               }
               response = await customerService.DeleteCustomerById(customerId);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Error : {ex.Message}");
            }
            return Ok(response);
        }
    }
}
