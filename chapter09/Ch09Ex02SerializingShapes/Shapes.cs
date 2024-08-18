using System.Xml.Serialization; // To use XMLAttribute
namespace Packt.Shared;

[XmlInclude(typeof(Circle))]
[XmlInclude(typeof(Rectangle))]
public class Shape
{
    public Shape() { }

    [XmlAttribute("shapeColor")]
    public string Colour { get; set; }

    [XmlAttribute("shapeArea")]
    public virtual double Area { get; set; }
}

#region Rectangle
public class Rectangle : Shape
{
    // constructor without perimeter
    public Rectangle() { }

    public double Height { get; set; }
    public double Width { get; set; }

    public override double Area { get { return base.Area = Height * Width; } }
}
#endregion

#region Circle 
public class Circle : Shape
{
    // constructor without perimeter
    public Circle() { }

    public double Radius { get; set; }

    public override double Area { get { return base.Area = Math.PI * Radius * Radius; } }


}
#endregion

