using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class DatabaseTableColumn
    {
        /// <remarks/>
        [XmlAttribute]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public SqlDbType SqlDbType { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore] 
        public bool IsIdentity { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsPk { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public int PkOrdinal { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public int MaxLength { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsComputed { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public byte Scale { get; set; }

        /// <remarks/>
        [XmlIgnore]
        [JsonIgnore]
        public byte Precision { get; set; }

        /// <remarks/>
        [XmlArrayItem("Transform", IsNullable = true)]
        public ObservableCollection<DatabaseTableColumnTransform> Transforms { get; set; } = new ObservableCollection<DatabaseTableColumnTransform>();

    }
}
