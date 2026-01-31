using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiTestCustomerCRUD.DTOs.Requests;
using WebApiTestCustomerCRUD.DTOs.Responses;
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
    }
}
