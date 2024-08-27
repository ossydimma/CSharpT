using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // TO use [Column]
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Northwind.EntityModels;
public class Category
{
    //these properties map to columns in the database
    public int CategoryId { get; set; } //primary key

    public string CategoryName { get; set; } = null!;

    [Column(TypeName = "ntext")]
    public string? Description { get; set; }

    // Defines a navigation property for related rows.

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; }
        // To enable developers to add products to a Category, we must
        // initialize the navigation property to an empty collection.
        // This also avoids an exception if we get a member like Count.
        = new HashSet<Product>();

}

