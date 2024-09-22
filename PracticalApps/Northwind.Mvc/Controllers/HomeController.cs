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

        public HomeController(ILogger<HomeController> logger, NorthwindContext db)
        {
            _logger = logger;
            _db = db;
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
