using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Database
    {
        /// <remarks/>
        [XmlAttribute]
        public string ServerName { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public int Port { get; set; } = 1433;

        /// <remarks/>
        [XmlAttribute]
        public string DatabaseName { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public string CredentialsName { get; set; }

        [XmlAttribute]
        public string Locale { get; set; }

        /// <remarks/>
        [XmlArrayItem("Table", IsNullable = false)]
        public List<DatabaseTable> Tables { get; set; } = new List<DatabaseTable>(); 

        /// <remarks/>
        [XmlArrayItem("Script", IsNullable = true)]
        public List<SqlScript> SqlScripts { get; set; } = new List<SqlScript>();

        /// <remarks/>
        [XmlArrayItem("ScriptingImport", IsNullable = true)]
        public List<ScriptingImport> ScriptingImports { get; set; } = new List<ScriptingImport>();

    }
}
