using System.Text.RegularExpressions;

//Requesting for a Regex
Write("Enter a regular expression (or press ENTER to use the default): ");
string regChecker = ReadLine()!;

Regex regex = new(); 
if (regChecker != null)
{
    regex = new(@"^\d+$");
} else
{
    regex = new(regChecker);

}


//Requesting for an input
Write("Enter Some Input: ");
string input = ReadLine()!;

WriteLine($"{input} matches {regChecker} ? {regex.IsMatch(input)}");






