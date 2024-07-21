using System.Globalization; // To use CultureInfo.
OutputEncoding = System.Text.Encoding.UTF8; // Enable Euro symbol.


string city = "London";
WriteLine($"{city} is {city.Length} characters long.");

//write the characters at the first and fourth positions in the string variable
WriteLine($"First char is {city[0]} and fourth is {city[3]}.");

//split method with the ',' argument
string cities = "Paris,Tehran,Chennai,Sydney,New York,Medellín";
string[] citiesArray = cities.Split(',');
WriteLine($"There are {citiesArray.Length} items in the array:");
foreach (string item in citiesArray)
{
    WriteLine($" {item}");
}

//SubString method
string fullName = "Alan Shore";
int indexOfTheSpace = fullName.IndexOf(' ');
string firstName = fullName.Substring(startIndex: 0, length: indexOfTheSpace);
string lastName = fullName.Substring(startIndex: indexOfTheSpace + 1);
WriteLine($"Original: {fullName}");
WriteLine($"Swapped: {lastName}, {firstName}");


//Checking a string for content
string company = "Microsoft";
WriteLine($"Text: {company}");
WriteLine("Starts with M: {0}, contains an N: {1}, ends with T:{2}",
 arg0: company.StartsWith("M"),
 arg1: company.Contains("N"),
 arg2: company.EndsWith("t"));

//comparing strings
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
string text1 = "Mark";
string text2 = "MARK";
WriteLine($"text1: {text1}, text2: {text2}");
WriteLine("Compare: {0}.", string.Compare(text1, text2)); // case not ignored
WriteLine("Compare (ignoreCase): {0}.",
 string.Compare(text1, text2, ignoreCase: true)); // case ignored
WriteLine("Compare (InvariantCultureIgnoreCase): {0}.",
 string.Compare(text1, text2,
 StringComparison.InvariantCultureIgnoreCase)); // culture and case invariant

//join method using "=>" as argument
string recombined = string.Join(" => ", citiesArray);
WriteLine(recombined);

//string.format 
string fruit = "Apples";
decimal price = 0.39M;
DateTime when = DateTime.Today;
WriteLine($"Interpolated: {fruit} cost {price:C} on {when:dddd}.");
WriteLine(string.Format("string.Format: {0} cost {1:C} on {2:dddd}.",
 arg0: fruit, arg1: price, arg2: when)); // Console.WriteLine support string.format, so this same code can be simplied as showed below
//WriteLine("WriteLine: {0} cost {1:C} on {2:dddd}.", arg0: fruit, arg1: price, arg2: when);

