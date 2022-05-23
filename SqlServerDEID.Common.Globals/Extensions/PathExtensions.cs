using System;
using System.IO;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static string GetPath(this string path, bool checkForExistence = true, string basePath = "")
        {
            if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path)); }

            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Path.GetDirectoryName(typeof(Extensions).Assembly.Location);
            }
            else
            {
                basePath = Path.GetDirectoryName(basePath);
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(basePath, path);
            }

            if (!File.Exists(path) && checkForExistence)
            {
                throw new FileNotFoundException($"The file:'{path}' does not exist");
            }

            return path;
        }
    }
}
