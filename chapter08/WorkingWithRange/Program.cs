string name = "Osita Jerry";

//Getting the length of the first and the last names
int lengthOfFirst = name.IndexOf(' ');
int lengthofLast = name.Length - lengthOfFirst - 1;

//using subset
string firstName = name.Substring(startIndex: 0, length: lengthOfFirst);
string lastName = name.Substring(startIndex: name.Length - lengthofLast, length: lengthofLast);

Console.WriteLine($"First: {firstName}, Last: {lastName}");

//using Span
ReadOnlySpan<char> nameAsSpan = name.AsSpan();
ReadOnlySpan<char> firstNameSpan = nameAsSpan[..lengthOfFirst];
ReadOnlySpan<char> lastNameSpan = nameAsSpan[^lengthofLast..];

Console.WriteLine($"First: {firstNameSpan}, last: {lastNameSpan}");