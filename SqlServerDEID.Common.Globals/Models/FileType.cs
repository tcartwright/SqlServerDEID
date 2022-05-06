using System.Text.Json.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FileType
    {
        xml,
        json
    }
}
