partial class Program 
{
    private static void DeferredExecution(string[] names)
    {
        SectionTitle("Deferred Execution");

        // Question: Which names end with an M?
        // (using a LINQ extension method)
        var query1 = names.Where(n => n.EndsWith("m"));

        // Question: Which names end with an M?
        // (using LINQ query comprehension syntax)
        var query2 = from name in names where name.EndsWith("m") select name;

        string[] result1 = query1.ToArray();

        List<string> result2 = query2.ToList();

        foreach(string name in query1)
        {
            WriteLine(name);
            names[2] = "Jimmy";
        }
    }

    private static void FilteringUsingWhere(string[] names)
    {
        SectionTitle(" Filtering entities using Where");

        // Explicitly creating required delegate
        //var query = names.Where(new Func<string, bool>(NameLongerThanFour));

        //the compiler creates the delegate automatically
        //var query = names.Where(NameLongerThanFour);

        // Using lambda experssion instead of a named method
        IOrderedEnumerable<string> query = names
            .Where(name => name.Length > 4)
            .OrderBy(name => name.Length) 
            .ThenBy(name => name);

        foreach (string item in query)
        {
            WriteLine(item);
        }
    }

    static void FilteringByType()
    {
        SectionTitle("Filtering by type");
        List<Exception> exceptions = new()
        {
            new ArgumentException(), new SystemException(),
            new IndexOutOfRangeException(), new InvalidOperationException(),
            new NullReferenceException(), new InvalidCastException(),
            new OverflowException(), new DivideByZeroException(),
            new ApplicationException()
        };

        IEnumerable<ArithmeticException> arithmeticExceptionsQuery = exceptions
            .OfType<ArithmeticException>();

        foreach(ArithmeticException e  in arithmeticExceptionsQuery)
        {
            WriteLine(e);
        }


    }
}

