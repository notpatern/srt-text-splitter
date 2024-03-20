using System.Text.Json.Nodes;

namespace srt_text_splitter.Services;

public class TextBatcher
{
    public JsonObject Execute(int maxWordsPerBatch, JsonObject json)
    {
        // TODO: If first file, do max words per max words and populate something with the time codes used If not, check between those time codes and batch the text
    }
}