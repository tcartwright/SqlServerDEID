using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{

    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SqlScript
    {
        /// <remarks/>
        [XmlAttribute]
        public string FileName { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public ScriptType ScriptType { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds for the sql script. Defaults to 30 seconds.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        [XmlAttribute]
        public int Timeout { get; set; } = 30;
    }
}
