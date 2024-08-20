using System.ComponentModel.DataAnnotations; // To use [Requird]
using System.ComponentModel.DataAnnotations.Schema; // To use [column]

namespace Northwind.EntityModels;
public class Product
{
    public int ProductId { get; set; } // Primary key

    [Required]
    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    //property name is different from column name 
    [Column("UnitPrice", TypeName = "money")]
    public decimal? Cost { get; set; }

    [Column("UnitsInStock")]
    public short? Stocks { get; set; }

    public bool Discontinued { get; set; }

    //these two properties are foregin key relationship to category table

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

}
