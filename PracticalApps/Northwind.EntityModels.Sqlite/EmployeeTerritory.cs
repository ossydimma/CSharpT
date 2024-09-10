using System.ComponentModel.DataAnnotations; // to use Attributes like [required]...
using System.ComponentModel.DataAnnotations.Schema; // To use attributes like [column]
using Microsoft.EntityFrameworkCore;

namespace Northwind.EntityModels;

public class EmployeeTerritory 
{
    [Column(TypeName = "INT")]
    public int EmployeeID { get; set; } 

    [Required]
    [Column(TypeName = "nvarchar] (20")]
    public string TerritoryID { get; set; } = null!;

}