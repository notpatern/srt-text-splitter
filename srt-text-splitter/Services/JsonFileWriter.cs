using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace srt_text_splitter.Services;

public class JsonFileWriter
{
    public void Execute(JsonObject json, string outputPath, string fileName)
    {
        string jsonToString = JsonSerializer.Serialize(json, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        
        StreamWriter streamWriter = new StreamWriter(outputPath + Path.DirectorySeparatorChar + fileName + ".json", false, Encoding.UTF8);
        streamWriter.Write(jsonToString);
        streamWriter.Close();
    }
}