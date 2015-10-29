using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automerge
{
    static class StreamCompareExctensions
    {
        public static string ReadComparableUnit(this StreamReader stream)
        {
            return stream.ReadLine().Trim();
        }

        public static IEnumerable<string> ReadAllComparableUnits(this StreamReader stream)
        {
            return Program.GenerateCollection(() => !stream.EndOfStream, () => stream.ReadComparableUnit());
        }
    }
}
