using System.Text.Json.Nodes;

namespace srt_text_splitter.Helper;

public class PreviousTimeCodesFinder
{
    public int[] Execute(double[] inOutTimeCode, JsonObject json)
    {
        int[] timeCodes = new int[2] { 0, 0 };

        double inTimeCode = inOutTimeCode[0];
        double outTimeCode = inOutTimeCode[1];
        
        bool firstChecked = false;

        for (int keyIndex = 1; keyIndex <= json.Count; keyIndex++)
        {
            string key = keyIndex.ToString();
            int oldKeyIndex = keyIndex - 1;
            double checkInTimeCode = Int32.Parse((string)json[key]["In"]);
            double checkOutTimeCode = Int32.Parse((string)json[key]["Out"]);

            if (checkInTimeCode >= inTimeCode && !firstChecked)
            {
                timeCodes[0] = keyIndex;
                firstChecked = true;
                continue;
            }

            if (!(checkOutTimeCode > outTimeCode))
            {
                continue;
            }
            
            timeCodes[1] = oldKeyIndex;
            break;
        }
        return timeCodes;
    }
}