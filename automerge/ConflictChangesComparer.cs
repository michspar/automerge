using System;
using System.Collections.Generic;

namespace automerge
{
    internal class ConflictChangesComparer : IEqualityComparer<Tuple<int, string, string>>
    {
        public bool Equals(Tuple<int, string, string> x, Tuple<int, string, string> y)
        {
            return x.Item1.Equals(y.Item1);
        }

        public int GetHashCode(Tuple<int, string, string> obj)
        {
            return obj.Item1.GetHashCode();
        }
    }
}