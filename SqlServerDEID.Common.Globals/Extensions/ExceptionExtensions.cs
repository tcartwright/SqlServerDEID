using System;
using System.IO;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static Exception GetRootInnerException(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
