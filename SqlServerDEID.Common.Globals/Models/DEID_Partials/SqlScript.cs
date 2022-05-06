using SqlServerDEID.Common.Globals.Extensions;
using System.IO;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class SqlScript
    {
        public string GetScript()
        {
            if (string.IsNullOrWhiteSpace(this.FileName)) { return null; }

            var path = this.FileName.GetPath();

            return File.ReadAllText(path);
        }
    }

}
