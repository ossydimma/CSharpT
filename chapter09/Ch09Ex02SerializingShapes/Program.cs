using Packt.Shared;
using System.Xml.Serialization;

// Create a list of Shapes to serialize.
List<Shape> listOfShapes = new()
{
 new Circle { Colour = "Red", Radius = 2.5 },
 new Rectangle { Colour = "Blue", Height = 20.0, Width = 10.0 },
 new Circle { Colour = "Green", Radius = 8.0 },
 new Circle { Colour = "Purple", Radius = 12.3 },
 new Rectangle { Colour = "Blue", Height = 45.0, Width = 18.0 }
};

XmlSerializer xs = new (type: listOfShapes.GetType());

string path = Path.Combine(CurrentDirectory, "shape.xml");

using(FileStream stream = File.Create(path))
{
    xs.Serialize(stream, listOfShapes);

}

WriteLine("File Info");
WriteLine($"File: {GetFileName(path)}");
WriteLine($"Path: {GetDirectoryName(path)}");
WriteLine($"Size: {new FileInfo(path).Length:N0} bytes.");
WriteLine("--------------------------");
WriteLine(File.ReadAllText(path));
WriteLine("--------------------------");

using(FileStream fileXml = File.Open(path, FileMode.Open))
{
    List<Shape>? loadedShape = xs.Deserialize(fileXml) as List<Shape>;

    foreach(Shape s in loadedShape)
    {
        WriteLine("{0} is {1} and has an area of {2:N2}", s.GetType().Name, s.Colour, s.Area);
    }
}

