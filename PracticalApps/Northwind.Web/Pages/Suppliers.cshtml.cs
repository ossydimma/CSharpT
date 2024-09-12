using Microsoft.AspNetCore.Mvc.RazorPages; // To use PageModel.
using Northwind.EntityModels; // To use NorthwindContext
using Microsoft.AspNetCore.Mvc; // To use [BlindProperty] and IActionResult
namespace Northwind.Web.Pages;
public class SuppliersModel : PageModel
{
    private NorthwindContext _db;

    [BindProperty]
    public Supplier? Supplier { get; set; }
    public SuppliersModel(NorthwindContext db)
    {
        _db = db;
    }
    public IEnumerable<Supplier>? Suppliers { get; set; }
    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Suppliers";
        //Suppliers = ["Alpha Co", "Beta Limited", "Gamma Corp"]

        Suppliers = _db.Suppliers
            .Distinct()
            .OrderBy(c => c.Country)
            .ThenBy(c => c.CompanyName);

    }

    public IActionResult OnPost()
    {
        if (Supplier is not null && ModelState.IsValid)
        {
            _db.Suppliers.Add(Supplier);
            _db.SaveChanges();
            return RedirectToPage("/Suppliers");
        }
        else
        {
            return Page(); // return to original page
        }
    }
}

