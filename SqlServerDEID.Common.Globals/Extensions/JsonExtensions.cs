using System;
using System.IO;
using System.Text.Json;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
	{
		public static T FromJson<T>(this string json)
		{
			var options = new JsonSerializerOptions()
			{
				MaxDepth = 64
			};
			return JsonSerializer.Deserialize<T>(json, options);
		}

		public static string ToJson(this object source)
		{
			return JsonSerializer.Serialize(source);
		}
	}
}
