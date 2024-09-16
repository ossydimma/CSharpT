using Microsoft.AspNetCore.Mvc; // To use [Route] [APiController], ControllerBase and so on
using Northwind.WebApi.Repositories; // To use IcustomerRespository
using Northwind.EntityModels;

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerRepository _repo;

        public CustomersController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        //// GET: api/customers
        //// GET: api/customers/?country=[country]
        //// this will always return a list of customers (but it might be empty)
        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        //public async Task<IEnumerable<Customer>> GetCustomers(string? country)
        //{
        //    if (string.IsNullOrWhiteSpace(country))
        //    {
        //        return await _repo.RetrieveAllAsync();
        //    }
        //    else
        //    {
        //        return (await _repo.RetrieveAllAsync())
        //        .Where(customer => customer.Country == country);
        //    }
        //}

        // GET: api/customers
        // GET: api/customers/?country=[country]
        // this will always return a list of customers (but it might be empty)
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public async Task<IEnumerable<Customer>> GetCustomers(string? country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return await _repo.RetrieveAllAsync();
            }
            else
            {
                return (await _repo.RetrieveAllAsync())
                    .Where(c => c.Country == country);
            }
        }

        //Get: api/customer/[id]
        [HttpGet("{id}", Name = nameof(GetCustomer))]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCustomer(string id)
        {
            Customer? c = await _repo.RetrieveAsync(id);
            if (c is null)
            {
                return NotFound(); // 404 Resources not found 
            }
            return Ok(c); // 200 OK with customer body
        }
        // POST: api/Customers
        //BODY: Customer (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Customer c)
        {
            if (c is null)
            {
                return BadRequest();
            }

            Customer? addedCustomer = await _repo.CreateAsync(c);
            if (addedCustomer is null)
            {
                return BadRequest("Repository failed to create customer");
            }
            else
            {
                return CreatedAtRoute( // 201 created
                    routeName: nameof(GetCustomer),
                    routeValues: new { id = addedCustomer.CustomerId.ToLower() },
                    value: addedCustomer);
            }
        }

        //PUT: api/customer/{id}
        //BODY: Customer (JSON. XML)
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, [FromBody]Customer c)
        {
            id = id.ToUpper();
            c.CustomerId = c.CustomerId.ToUpper();

            if(c is null || c.CustomerId != id)
            {
                return BadRequest(); // 400
            }
            Customer? existing = await _repo.RetrieveAsync(id);

            if(existing is null)
            {
                return NotFound(); // 404 resource not found
            }
            await _repo.UpdateAsync(c);
            return new NoContentResult(); // 204 no content

        }

        //DELETE: api/customer/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]


        public async Task<IActionResult> Delete(string id)
        {
            // Take control of problem details.
            if (id == "bad")
            {
                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://localhost:5151/customers/failed-to-delete",
                    Title = $"Customer ID {id} found but failed to delete.",
                    Detail = "More details like Company Name, Country and so on.",
                    Instance = HttpContext.Request.Path
                };
                return BadRequest(problemDetails); // 400 Bad Request

            }

                Customer? existing = await _repo.RetrieveAsync(id);

            if(existing is null)
            {
                return NotFound(); // 404 
            }

            bool? deleted = await _repo.DeleteAsync(id);
            if(deleted.HasValue && deleted.Value) // short circuit AND
            {
                return new NoContentResult(); // 204 no content
            }
            else
            {
                return BadRequest($"customer {id} was found but fail to delete"); // 400 Bad request
            }
        }


    }
}
