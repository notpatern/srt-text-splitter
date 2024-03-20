using System.Text.Json.Nodes;
using srt_text_splitter.Services;

namespace srt_text_splitter;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        List<string> files = new List<string>();
        SrtParser srtParser = new SrtParser();
        JsonFileWriter jsonFileWriter = new JsonFileWriter();

        string outputPath = args[0];

        for (int argumentFileIndex = 1; argumentFileIndex < args.Length; argumentFileIndex++)
        {
            files.Add(args[argumentFileIndex]);
        }
        
        foreach (string file in files)
        {
            Console.WriteLine($"Started parsing '{Path.GetFileName(file)}' to a JsonArray...");
            JsonObject json = srtParser.Execute(file);
            Console.WriteLine($"Parsing done.");
            
            Console.WriteLine($"Started writing .json file for '{Path.GetFileName(file)}'...");
            jsonFileWriter.Execute(json, outputPath, Path.GetFileNameWithoutExtension(file));
            Console.WriteLine($"File successfully written.");
        }
    }
}
