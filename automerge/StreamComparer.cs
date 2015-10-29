using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace automerge
{
    internal class StreamComparer
    {
        private StreamReader stream1;
        private StreamReader stream2;

        public StreamComparer(FileStream fileStream1, FileStream fileStream2)
        {
            this.stream1 = new StreamReader(fileStream1);
            this.stream2 = new StreamReader(fileStream2);
        }

        public Tuple<int, string, string>[] Compare()
        {
            var changes = new List<Tuple<int, string, string>>();

            if (stream1.EndOfStream && stream2.EndOfStream)
                return changes.ToArray();

            if (stream1.EndOfStream)
                return stream2.ReadAllComparableUnits().Select((ch, i) => Tuple.Create(i, "", ch)).ToArray();
            
            if (stream2.EndOfStream)
                return stream1.ReadAllComparableUnits().Select((ch, i) => Tuple.Create(i, ch, "")).ToArray();

            var line1 = stream1.ReadComparableUnit();
            var line2 = stream2.ReadComparableUnit();

            while (!stream1.EndOfStream && !stream2.EndOfStream && line1 == line2)
            {
                line1 = stream1.ReadComparableUnit();
                line2 = stream2.ReadComparableUnit();
            }

            return changes.ToArray();
        }
    }
}