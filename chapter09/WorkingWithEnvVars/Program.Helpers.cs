
partial class Program
{ 
    private static void SectionTitle(string title)
    {
        ConsoleColor prev = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine($"*** {title} ***");
        ForegroundColor = prev;
    }

    private static void DictionaryToTable(IDictionary dictionary)
    {
        Table table = new();
        table.AddColumn("key");
        table.AddColumn("value");

        foreach (string key in dictionary.Keys)
        {
            table.AddRow(key, dictionary[key]!.ToString()!);
        }

        AnsiConsole.Write(table);
    }
}
