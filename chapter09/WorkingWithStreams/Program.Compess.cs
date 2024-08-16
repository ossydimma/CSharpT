using Packt.Shared; // To use Viper.
using System.IO.Compression; // To use BrotliStream, GZipStream.
using System.Xml; // To use XmlWriter, XmlReader.

partial class Program
{
    private static void Compress(string algorithm = "gzip")
    {
        // Define a file path using the algorithm as file extension.
        string filePath = Combine(CurrentDirectory, $"streams.{algorithm}");

        FileStream file = File.Create( filePath );

        Stream compressor;

        if (algorithm == "gzip")
        {
            compressor = new GZipStream( file, CompressionMode.Compress );
        }
        else
        {
            compressor = new BrotliStream( file, CompressionMode.Compress );
        }

        using (compressor)
        {
            using (XmlWriter xml = XmlWriter.Create( compressor ) )
            {
                xml.WriteStartDocument();
                xml.WriteStartElement( "callsigns" );

                foreach (string item in Viper.Callsigns )
                {
                    xml.WriteElementString("callsing", item);
                }
            }
        } // also closes the underlying stream

        OutputFileInfo(filePath);

        //Readin the compressed file
        WriteLine("Reading the compressed Xml file");
        file = File.Open(filePath, FileMode.Open);
        
        Stream decompressor;

        if (algorithm == "gzip")
        {
            decompressor = new GZipStream(file, CompressionMode.Decompress );
        }
        else
        {
            decompressor = new BrotliStream(file, CompressionMode.Decompress );
        }

        using (decompressor)
        using (XmlReader reader = XmlReader.Create(decompressor)) 

        while (reader.Read())
        {
            if((reader.NodeType == XmlNodeType.Element) && (reader.Name == "callsigns"))
            {
                reader.Read(); // move to the next element
                WriteLine(reader.Value); // Read its value
            }

            // Alternative syntax with property pattern matching:
            // if (reader is { NodeType: XmlNodeType.Element,
            // Name: "callsign" })

        }

    }
}

