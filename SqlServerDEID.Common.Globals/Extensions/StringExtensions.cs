using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static string CleanName(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this)) { throw new System.ArgumentException($"'{nameof(@this)}' cannot be null or whitespace.", nameof(@this)); }

            @this = @this.Replace("[", "").Replace("]", "");
            return @this;
        }

        public static string FormatName(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this)) { return string.Empty; }
            if (@this.StartsWith("#")) { return @this; }

            @this = @this.CleanName();
            var parts = @this.Split('.');
            if (parts.Length < 2)
            {
                throw new ApplicationException($"The table '{@this}' is not using two part naming. Tables must be named with both schema and object name.");
            }
            return $"[{string.Join("].[", parts)}]".ToUpper();
        }

    }
}
