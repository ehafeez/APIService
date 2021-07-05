using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Entities;
using Test.Requests;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController<CustomerController>
    {
        private readonly ICustomerService _customerService;
        private List<CustomerEntity> existingCustomers = new List<CustomerEntity>();        
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
            existingCustomers = _customerService.LoadData();
        }

        /// <summary>
        /// Get the customers list
        /// </summary>
        [HttpGet, ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("getCustomers")]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (existingCustomers?.Count == 0)
                    return NoContent();

                return Ok(existingCustomers);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error occurred while getting a customer: {ex}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add the customers in the list
        /// </summary>
        /// <param name="customerVm"></param>
        /// <returns></returns>
        [HttpPost, ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("addCustomer")]
        public async Task<IActionResult> Post([FromBody] List<CustomerViewModel> customerVm)
        {
            try
            {
                if (!ModelStateIsValid())
                    return BadRequest();

                var result = new List<CustomerEntity>();
                var customerRequest = Mapper.Map<List<CustomerEntity>>(customerVm);

                customerRequest?.ForEach(cust =>
                {
                    if (_customerService.CheckIdExists(existingCustomers, cust.Id))
                        throw new Exception("Id field must be unique");

                    existingCustomers = _customerService.LoadData();
                    result = _customerService.SortData(existingCustomers, cust);
                    _customerService.SaveData(result);
                });

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error occurred while adding a customer: {ex}");
                return BadRequest(ex.Message);
            }
        }
    }
}