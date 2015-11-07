using System;
using System.Collections.Generic;

namespace automerge
{
    internal class ConflictChangesComparer : IEqualityComparer<Tuple<int, int, string, string>>
    {
        public bool Equals(Tuple<int, int, string, string> x, Tuple<int, int, string, string> y)
        {
            return x.Item1.Equals(y.Item1);
        }

        public int GetHashCode(Tuple<int, int, string, string> obj)
        {
            return obj.Item1.GetHashCode();
        }
    }
}