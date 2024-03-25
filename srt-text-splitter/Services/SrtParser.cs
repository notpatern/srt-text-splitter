using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace srt_text_splitter.Services;

public class SrtParser()
{
    public JsonObject Execute(string file)
    {
        JsonObject json = new JsonObject();
        string[] fileToString = File.ReadAllLines(file);

        json = PopulateJsonArray(fileToString, json);
        
        return json;
    }

    private JsonObject PopulateJsonArray(string[] fileToString, JsonObject json)
    {
        for (int lineIndex = 0; lineIndex < fileToString.Length; lineIndex++)
        {
            if (!fileToString[lineIndex].Contains("-->"))
            {
                continue;
            }
            
            string key = fileToString[lineIndex - 1];

            Dictionary<string, string> timeCodes = TimeCodesToMilliseconds(fileToString[lineIndex]);
            string text = TextLinesToSingleString(fileToString, lineIndex + 1);
            
            Dictionary<string, string> valueDictionary =
                new Dictionary<string, string>()
                {
                    ["In"] = timeCodes["In"],
                    ["Out"] = timeCodes["Out"],
                    ["Content"] = text
                    
                };
            
            string valueString = JsonSerializer.Serialize(valueDictionary,
                new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            
            json?.Add(key, JsonNode.Parse(valueString));
        }
        
        return json;
    }

    private Dictionary<string, string> TimeCodesToMilliseconds(string timecodesLine)
    {
        string firstTimeCode = "";
        string secondTimeCode = "";
        bool firstChecked = false;
        
        foreach (char character in timecodesLine)
        {
            if (Char.IsWhiteSpace(character) || Char.IsSymbol(character) || character == '-')
            {
                firstChecked = true;
                continue;
            }

            if (firstChecked)
            {
                secondTimeCode += character;
                continue;
            }

            firstTimeCode += character;
        }
        
        TimeSpan firstTimeSpan = TimeSpan.ParseExact(firstTimeCode, 
            @"hh\:mm\:ss\,fff", null);
        
        TimeSpan secondTimeSpan = TimeSpan.ParseExact(secondTimeCode, 
            @"hh\:mm\:ss\,fff", null);

        string firstTimeCodeMs = firstTimeSpan.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        string secondTimeCodeMs = secondTimeSpan.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);

        Dictionary<string, string> timeCodes = new Dictionary<string, string>
        {
            ["In"] = firstTimeCodeMs,
            ["Out"] = secondTimeCodeMs
        };

        return timeCodes;
    }

    private string TextLinesToSingleString(string[] fileToString, int textFirstLineIndex)
    {
        string text = "";
        for (int textLineIndex = textFirstLineIndex; textLineIndex < fileToString.Length; textLineIndex++)
        {
            if (fileToString[textLineIndex].Length == 0)
            {
                break;
            }

            text += fileToString[textLineIndex];
            text += " ";
        }

        return text;
    }
}