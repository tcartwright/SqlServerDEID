using System.Collections.Generic;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class DatabaseTable
    {
        /// <remarks/>
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool DisableTriggers { get; set; }

        [XmlAttribute]
        public bool DisableConstraints { get; set; }

        /// <summary>
        /// Gets or sets the script timeout in seconds for enabling or disabling constraints and triggers. Defaults to 180 seconds or three minutes.
        /// </summary>
        /// <value>
        /// The script timeout.
        /// </value>
        [XmlAttribute]
        public int ScriptTimeout { get; set; } = 180;

        /// <remarks/>
        [XmlArrayItem("Column", IsNullable = false)]
        public List<DatabaseTableColumn> Columns { get; set; } = new List<DatabaseTableColumn>();

        /// <remarks/>
        [XmlArrayItem("Script", IsNullable = true)]
        public List<SqlScript> SqlScripts { get; set; } = new List<SqlScript>();

    }
}
