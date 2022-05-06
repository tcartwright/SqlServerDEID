using System.Text.Json.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransformType
    {
        expression,
        powershell
    }
}
