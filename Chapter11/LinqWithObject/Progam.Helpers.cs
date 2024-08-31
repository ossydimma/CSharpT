
partial class Program 
{
    public static void SectionTitle(string title)
    {
        ConsoleColor prev = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine($"*** {title} ***");
        ForegroundColor = prev;
    }
}

