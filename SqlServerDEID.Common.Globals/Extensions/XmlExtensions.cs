using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Extensions
{
    [DebuggerStepThrough]
    public static partial class Extensions
    {
        public static string ToXml(this object source)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None,
                Indent = false
            };

            var stringWriter = new StringWriter();
            var writer = XmlWriter.Create(stringWriter, settings);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var serializer = new XmlSerializer(source.GetType());

            serializer.Serialize(writer, source, namespaces);

            return stringWriter.ToString();
        }

        public static T FromXml<T>(this string xml) where T : class
        {
            if (string.IsNullOrWhiteSpace(xml)) { throw new ArgumentException($"'{nameof(xml)}' cannot be null or whitespace.", nameof(xml)); }

            var serializer = new XmlSerializer(typeof(T), new[] { typeof(JsonProperty), typeof(JsonIgnoreAttribute) } );
            
            using (TextReader reader = new StringReader(xml))
            {
                return serializer.Deserialize(reader) as T;
            }
        }
    }
}
