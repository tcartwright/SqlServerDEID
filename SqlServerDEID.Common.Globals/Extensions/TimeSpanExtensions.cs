using System;
using System.IO;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static string ToStringFormat(this TimeSpan timespan, string format = @"d\.hh\:mm\:ss")
        {
            return timespan.ToString(format);
        }
    }
}
