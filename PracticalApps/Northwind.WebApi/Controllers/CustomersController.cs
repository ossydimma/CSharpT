using Microsoft.AspNetCore.Mvc; // To use [Route] [APiController], ControllerBase and so on
using Northwind.WebApi.Repositories; // To use IcustomerRespository
using Northwind.EntityModels; // To use Customer

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerRepository _repo;

        public  CustomersController (ICustomerRepository repo)
        { 
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
