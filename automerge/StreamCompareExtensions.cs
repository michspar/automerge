using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automerge
{
    static class StreamCompareExtensions
    {
        public static string ReadComparableUnit(this StreamReader stream)
        {
            return stream.ReadLine().Trim();
        }

        public static IEnumerable<string> ReadAllComparableUnits(this StreamReader stream)
        {
            while (!stream.EndOfStream)
                yield return stream.ReadComparableUnit();
        }
    }
}
