namespace SqlServerDEID.Common.Globals.Models
{
    // This class will be passed into both the Powershell, and the C# scriptlets. So try to keep it a simple POCO
    public class ColumnInfo
    {
        public string Name { get; internal set; }
        public string DataType { get; internal set; }
        public int MaxLength { get; internal set; }
        public int Precision { get; internal set; }
        public int Scale { get; internal set; }

        public override string ToString()
        {
            return $"{Name,50} {DataType}";
        }
    }
}