using System.Text.Json.Nodes;

namespace srt_text_splitter.Structures;

public struct DataCollection(List<double[]>? batchTimeCodes = null, JsonObject? json = null)
{
    public List<double[]>? BatchTimeCodes { get; set; } = batchTimeCodes;
    public JsonObject? Json { get; set; } = json;
}