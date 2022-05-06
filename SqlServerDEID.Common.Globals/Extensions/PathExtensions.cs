using System;
using System.IO;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static string GetPath(this string path, bool checkForExistence = true)
        {
            if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path)); }

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Path.GetDirectoryName(typeof(Extensions).Assembly.Location), path);
            }

            if (!File.Exists(path) && checkForExistence)
            {
                throw new FileNotFoundException($"The file:'{path}' does not exist");
            }

            return path;
        }
    }
}
