using System.ComponentModel.DataAnnotations; // To use [required] and [strengthLength].

namespace Northwind.EntityModel;

public class Customer 
{
    //Primary Key
    public string CustomerID { get; set; } = null!;

    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    public string City { get; set; } = null!;


}

