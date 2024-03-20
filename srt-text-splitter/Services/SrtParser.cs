using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace srt_text_splitter.Services;

public class SrtParser()
{
    public JsonObject Execute(string file)
    {
        JsonObject json = new JsonObject();
        string[] fileToString = File.ReadAllLines(file);

        json = PopulateJsonArray(ref fileToString, json);
        
        return json;
    }

    private JsonObject PopulateJsonArray(ref string[] fileToString, JsonObject json)
    {
        for (int lineIndex = 0; lineIndex < fileToString.Length; lineIndex++)
        {
            string key;
            (double, double) timeCodes;
            string text;

            if (!fileToString[lineIndex].Contains("-->"))
            {
                continue;
            }
            
            key = fileToString[lineIndex - 1];
            timeCodes = TimeCodesToMilliseconds(fileToString[lineIndex]);
            text = TextLinesToSingleString(fileToString, lineIndex + 1);
            
            string value = JsonSerializer.Serialize((timeCodes, text), new JsonSerializerOptions { IncludeFields = true, WriteIndented = true});

            json.Add(key, JsonNode.Parse(value));
        }
        
        

        return json;
    }

    private (double, double) TimeCodesToMilliseconds(string timecodesLine)
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

        double firstTimeCodeMs = firstTimeSpan.TotalMilliseconds;
        double secondTimeCodeMs = secondTimeSpan.TotalMilliseconds;

        return (firstTimeCodeMs, secondTimeCodeMs);
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