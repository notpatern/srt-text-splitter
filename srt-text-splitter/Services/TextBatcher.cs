using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using srt_text_splitter.Structures;

namespace srt_text_splitter.Services;

public class TextBatcher
{
    public DataCollection Execute(int maxWordsPerBatch, JsonObject? json, List<double[]>? batchTimeCodes)
    {
        return batchTimeCodes != null ? RunBatchWithTimeCodes(batchTimeCodes, json) : RunBatchWithMaxWords(maxWordsPerBatch, json);
    }

    private DataCollection RunBatchWithTimeCodes(List<double[]>? batchTimeCodes, JsonObject? json)
    {
        return new DataCollection(batchTimeCodes, json);
    }

    private DataCollection RunBatchWithMaxWords(int maxWordPerBatch, JsonObject? json)
    {
        JsonObject newJson = new JsonObject();
        List<double[]> timeCodes = new List<double[]>();
        
        char[] delimiters = [' ', '\r', '\n'];
        int wordAmount = 0;
        
        int batchKey = 1;
        double batchIn = (double)Int32.Parse((string)json["1"]!["In"]!);
        string batchContent = "";

        for (int index = 1; index <= json.Count; index++)
        {
            string keyIndex = index.ToString();
            wordAmount += json[keyIndex]!["Content"]!.ToString().Split(delimiters,StringSplitOptions.RemoveEmptyEntries).Length;
            
            if (wordAmount > maxWordPerBatch)
            {
                string previousKey = (index - 1).ToString();
                string nextKey = (index + 1).ToString();
                
                var batchOut = Int32.Parse((string)json[previousKey]!["Out"]!);
                string valueIn = batchIn.ToString(CultureInfo.InvariantCulture);
                string valueOut = batchOut.ToString(CultureInfo.InvariantCulture);
                string key = batchKey.ToString();
                
                Dictionary<string, string> valueDictionary =
                    new Dictionary<string, string>()
                    {
                        ["In"] = valueIn,
                        ["Out"] = valueOut,
                        ["Content"] = batchContent
                    };
                
                string valueString = JsonSerializer.Serialize(valueDictionary,
                    new JsonSerializerOptions { IncludeFields = true, WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
                
                JsonNode? value = JsonNode.Parse(valueString);
                
                newJson.Add(key, value);
                timeCodes.Add([batchIn, batchOut]);

                batchKey++;
                batchContent = "";
                wordAmount = 0;
                if (index != json.Count)
                {
                    batchIn = Int32.Parse((string)json[nextKey]!["In"]!);
                }
            }

            batchContent += json[keyIndex]!["Content"]!.ToString();
        }

        return new DataCollection(timeCodes, newJson);
    }
}