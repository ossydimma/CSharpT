using System.Xml.Serialization;
using fastJson = System.Text.Json.JsonSerializer; // to use Json serialzer
using System.Text.Json; // To use JsonSerializer.
using Packt.Shared;  // To use People and Book
using Newtonsoft.Json; 

List<Person> people = new()
{
    new(initialSalary: 30_000M)
    {
        FirstName = "Alice",
        LastName = "Smith",
        DateOfBirth = new(year: 1979, month: 3, day:14)
    },
    new(initialSalary: 40_000M)
    { 
        FirstName = "Bob",
        LastName = "Jones",
        DateOfBirth = new(year: 1969, month: 11,  day: 23)
    
    },
    new(initialSalary: 20_000M)
    {
        FirstName = "Charile",
        LastName = "Cox",
        DateOfBirth = new(year: 1984, month: 5, day: 11),
        Children = new()
        { 
            new(initialSalary: 0M)
            {
                FirstName = "Sally",
                LastName = "Cox",
                DateOfBirth = new(year: 2020, month: 10, day: 17)
            }
        }
    }
};

Person lastBorn = new(initialSalary: 0M)
{
    FirstName = "James",
    LastName = "Brown",
    DateOfBirth = new(year: 2012, month: 10, day: 12)
};

people.Add(lastBorn);

#region Serialization as XMl

SectionTitle("Serialization as XMl");

XmlSerializer xs = new(type: people.GetType());

WriteLine($"xs : {xs}");

// Create a file to write to.
string path = Combine(CurrentDirectory, "people.xml");

using(FileStream stream = File.Create(path))
{
    //serialize the object graph to  stream
    xs.Serialize(stream, people);
} // close stream

OutputFileInfo(path);

SectionTitle("Deserializing xml file");

using(FileStream xmlLoaded = File.Open(path, FileMode.Open))
{
    List<Person>? loadedPeople = xs.Deserialize(xmlLoaded) as List<Person>;

    if(loadedPeople is not null)
    {
        foreach(Person p in loadedPeople)
        {
            WriteLine("{0} has {1} children. ", p.LastName, p.Children?.Count ?? 0);
        }
    }
}


#endregion

#region Serializing with Json 
SectionTitle("Serializing with Json");

// creating a file to write to
string jsonPath = Combine(CurrentDirectory, "people.json");

using (StreamWriter jsonStream = File.CreateText(jsonPath))
{
    Newtonsoft.Json.JsonSerializer jss = new();

    // Serialzing the object graph into string
    jss.Serialize(jsonStream, people);
} // closing file stream to relase resources

OutputFileInfo(jsonPath);

SectionTitle("Deserializing json File");

await using (FileStream jsonLoad = File.Open(jsonPath, FileMode.Open))
{
    List<Person>? loadedPeople = await fastJson.DeserializeAsync(utf8Json: jsonLoad, returnType: typeof(List<Person>)) as List<Person>;

    if (loadedPeople is not null)
    {
        foreach( Person p in loadedPeople)
        {
            WriteLine("{0} has {1} childeren. ", p.LastName, p.Children?.Count ?? 0);
        }
    }
}
#endregion

#region  Controlling Json Processing

Book Csharp = new("C# 12 and .NET 8 - Modern Cross-Platform Development Fundamentals")
{
    Author = "Osita Chris Jerry",
    PublishDate = new(year: 2023, month: 11, day: 14),
    Created = DateTimeOffset.UtcNow,
    Pages = 200
};
JsonSerializerOptions options = new()
{
    IncludeFields = true,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};

string controlPath = Combine(CurrentDirectory, "Book.json");

using(Stream fileStream = File.Create(controlPath))
{
    fastJson.Serialize(utf8Json : fileStream, value: Csharp, options);
}

OutputFileInfo(controlPath);

#endregion