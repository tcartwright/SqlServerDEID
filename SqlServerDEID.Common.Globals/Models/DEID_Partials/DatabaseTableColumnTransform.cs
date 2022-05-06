using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class DatabaseTableColumnTransform
    {
        /// <summary>
        /// This object will actually hold a Script<object> refererence from the Microsoft.CodeAnalysis.CSharp.Scripting namespace, but must be mapped as object so the reference does not need to be carried
        /// </summary>
        /// <value>
        /// The script.
        /// </value>
        [XmlIgnore]
        [JsonIgnore]
        public object Script { get; set; }

        public override string ToString()
        {
            return this.Transform;
        }
    }

}
