using System.Text.Json.Serialization; // To use [JsonInclude] attribute

using Packt.Shared;
public class Book 
{ 
    public Book(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
    public string? Author { get; set; }

    [JsonInclude] // To include field
    public DateTime PublishDate;

    [JsonInclude] // to include field
    public DateTimeOffset Created;


    public ushort Pages;

}
