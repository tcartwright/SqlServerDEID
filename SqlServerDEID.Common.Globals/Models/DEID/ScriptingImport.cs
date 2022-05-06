using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class ScriptingImport
    {
        [XmlAttribute]
        public string NameSpace { get; set; }
    }
}
