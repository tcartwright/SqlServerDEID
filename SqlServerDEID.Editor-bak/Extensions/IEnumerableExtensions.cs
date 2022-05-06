using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlServerDEID.Editor.Extensions
{
    public static partial class Extensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            if (sequences == null)
            {
                return null;
            }

            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) => accumulator.SelectMany(
                    accseq => sequence,
                    (accseq, item) => accseq.Concat(new[] { item })));
        }
    }
}
