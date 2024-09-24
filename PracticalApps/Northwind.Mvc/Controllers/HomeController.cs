using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using System.Diagnostics;
using Northwind.EntityModels; // To use NorthwindContext
using Microsoft.EntityFrameworkCore; // To use Include method.
using Microsoft.AspNetCore.Authorization; // To use [Authorize]

namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext _db;
        private readonly IHttpClientFactory _ClientFactory;

        public HomeController(ILogger<HomeController> logger, NorthwindContext db, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _db = db;
            _ClientFactory = httpClientFactory;
        }
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index() 
        {
            HomeIndexViewModel model = new
            (
                VisitorCount: Random.Shared.Next(1, 1000),
                Categories: await _db.Categories.ToListAsync(),
                Products: await _db.Products.ToListAsync()
            );

            try
            {
                HttpClient client = _ClientFactory.CreateClient(name: "Northwind.MinimalApi");

                HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: "todos");
                HttpResponseMessage response = await client.SendAsync(request);

                ViewData["todos"] = await response.Content.ReadFromJsonAsync<ToDo[]>();
            }
            catch(Exception ex)
            {
                _logger.LogWarning($"The Minimal.WebApi service is not responding. Exception: {ex.Message}");

                ViewData["todos"] = Enumerable.Empty<ToDo>().ToArray();
            }


            _logger.LogError("This is a serious error! (not reallu)");
            _logger.LogWarning("This the first warning!");
            _logger.LogWarning("second warning");
            _logger.LogInformation("I am in the Index method of the HomeController.");

            return View(model);
        }

        public async Task<IActionResult> ProductDetail(int? id, string alertStyle = "success")
        {
            ViewData["alertstyle"] = alertStyle;

            if(!id.HasValue)
            {
                return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }

            Product? model = await _db.Products.Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.ProductId == id);

            if(model == null)
            {
                return NotFound($"ProductId {id} not found.");
            }

            return View(model); // Pass model to view and then return result.
        }

        public async Task<IActionResult> CategoryDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass a category ID in the route, for example, /Home/CategoryDetail/6");
            }

            Category? model = await _db.Categories.Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.CategoryId == id);
            if (model is null)
            {
                return NotFound($"CategoryId {id} not found");
            }

            return View(model); 
        }

        public async Task<IActionResult> Customers(string? country)
        {
            string uri;

            if(string.IsNullOrEmpty(country))
            {
                uri = "api/customers";
                ViewData["Title"] = "All Customers worldwide";
            }
            else
            {
                uri = $"api/customers/?country={country}";
                ViewData["Title"] = $"Customers in {country}";
            }

            HttpClient client = _ClientFactory.CreateClient(name: "Northwind.WebApi");

            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: uri);
            HttpResponseMessage response = await client.SendAsync(request);

            IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();

            return View(model);

        }

        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
            if(!price.HasValue)
            {
                return BadRequest("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
            }

            IEnumerable<Product> model = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Where(p => p.UnitPrice > price);

            if(!model.Any())
            {
                return NotFound($"No products cost more than {price:C}.");
            }

            ViewData["MaxPrice"] = price.Value.ToString("c");

            return View(model);
        }

        // This action method will handle GET and other requests except POST.
        public IActionResult ModelBinding()
        {
            return View(); // The page with a form to submit.
        }

        [HttpPost] // This action method will handle POST requests
        public IActionResult ModelBinding(Thing thing)
        {
            HomeModelBindingViewModel model = new(
                Thing: thing,
                HasErrors: !ModelState.IsValid,
                ValidationErrors: ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage)
                );

            return View(model); // show the model bound thing
        }
        // GET /Home/AddCustomer
        public IActionResult AddCustomer()
        {
            ViewData["Title"] = "Add Customer";
            return View();

        }

        // POST /Home/AddCustomer
        // A Customer object in the request body.
        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {

            HttpClient client = _ClientFactory.CreateClient(name: "Northwind.WebApi");

            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/customers", value: customer);

            Customer? model = await response.Content
                .ReadFromJsonAsync<Customer>();

            if (response.IsSuccessStatusCode)
            {
                TempData["success-message"] = "Customer successfully added.";
            }
            else
            {
                TempData["error-message"] = "Customer was NOT added.";
            }

            // Show the full customers list to see if it was added.
            return RedirectToAction("customer");
        }
        //Get Home/DeleteCustomer/{customerId}
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            HttpClient client = _ClientFactory.CreateClient(name: "Northwind.WebApi");

            Customer? customer = await client.GetFromJsonAsync<Customer>(requestUri: $"api/customers/{customerId}");

            ViewData["Title"] = "Delete Customer";

            return View(customer);
        }

        // POST /Home/DeleteCustomer
        // A CustomerId in the request body e.g. ALFKI.
        [HttpPost]
        [Route("home/deletecustomer")]
        // Action method name must have a different name from the GET method
        // due to C# not allowing duplicate method signatures.
        public async Task<IActionResult> DeleteCustomerPost(string customerId)
        {
            HttpClient client = _ClientFactory.CreateClient(name: "Northwind.WebApi");

            HttpResponseMessage respone = await client.DeleteAsync(requestUri: $"api/customers/{customerId}");

            if (respone.IsSuccessStatusCode)
            {
                TempData["sucess-message"] = "Customer successfully deleted.";
            }
            else
            {
                TempData["error-message"] = $"Customer {customerId} was not deleted.";       
            }

            // Show the full customers list to see if it was deleted.
            return RedirectToAction("Customers");
        }



        [Route("private")]
        [Authorize(Roles = "Administrators")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
