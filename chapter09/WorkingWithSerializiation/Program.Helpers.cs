
partial class Program 
{
    private static void SectionTitle( string title )
    {
        ConsoleColor prevColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine( $"*** {title } ***");
        ForegroundColor = prevColor;
    }
    private static void TaglineTitle ( string tag )
    {
        ConsoleColor prev = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkBlue;
        WriteLine($"*** {tag} ***");
        ForegroundColor = prev;
    }
    private static void OutputFileInfo(string path)
    {
        TaglineTitle( "File Info");
        WriteLine($"File: {GetFileName(path)}");
        WriteLine($"Path: {GetDirectoryName(path)}");
        WriteLine($"Size: { new FileInfo(path).Length:N0} bytes.");
        WriteLine("--------------------------");
        WriteLine(File.ReadAllText(path));
        WriteLine("--------------------------");
    }

}

