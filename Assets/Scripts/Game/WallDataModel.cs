using System.Collections.Generic;
using System.Text.Json.Serialization;

public class WallDataModel
{
    [JsonPropertyName("data")]
    public List<int> Data;
}