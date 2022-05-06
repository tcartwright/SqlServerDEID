using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class DatabaseTableColumnTransform
    {
        /// <remarks/>
        [XmlAttribute]
        public string Transform { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public TransformType TransformType { get; set; }

        /// <remarks/>
        [XmlAttribute]
        public string WhereClause { get; set; }
    }
}
