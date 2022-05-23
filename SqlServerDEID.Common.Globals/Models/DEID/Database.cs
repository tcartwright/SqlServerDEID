using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [XmlAttribute]
        public string PreScript { get; set; }

        [XmlAttribute]
        public string PostScript { get; set; }

        [XmlAttribute]
        public int ScriptTimeout { get; set; } = 180;


        /// <remarks/>
        [XmlArrayItem("Table", IsNullable = false)]
        public ObservableCollection<DatabaseTable> Tables { get; set; } = new ObservableCollection<DatabaseTable>(); 

        /// <remarks/>
        [XmlArrayItem("ScriptingImport", IsNullable = true)]
        public ObservableCollection<ScriptingImport> ScriptingImports { get; set; } = new ObservableCollection<ScriptingImport>();

    }
}
