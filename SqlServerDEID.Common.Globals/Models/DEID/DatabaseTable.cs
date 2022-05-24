using SqlServerDEID.Common.Globals.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class DatabaseTable
    {
        private string _name;

        /// <remarks/>
        [XmlAttribute]
        public string Name { get => _name; set => _name = value.FormatName(); }

        [XmlAttribute]
        public bool DisableTriggers { get; set; } = true;

        [XmlAttribute]
        public bool DisableConstraints { get; set; }

        [XmlAttribute]
        public string PreScript { get; set; }

        [XmlAttribute]
        public string PostScript { get; set; }

        /// <summary>
        /// Gets or sets the script timeout in seconds for enabling or disabling constraints and triggers and the pre and post scripts. Defaults to 180 seconds or three minutes.
        /// </summary>
        /// <value>
        /// The script timeout.
        /// </value>
        [XmlAttribute]
        public int ScriptTimeout { get; set; } = 180;

        /// <remarks/>
        [XmlArrayItem("Column", IsNullable = false)]
        public ObservableCollection<DatabaseTableColumn> Columns { get; set; } = new ObservableCollection<DatabaseTableColumn>();
    }
}
