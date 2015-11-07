using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace automerge
{
    using Change = Tuple<int, int, string, string>;

    public class StreamComparer
    {
        private List<Tuple<int, string>> linesLeft, linesRight;

        public StreamComparer(Stream streamLeft, Stream streamRight)
        {
            linesLeft = new StreamReader(streamLeft).ReadAllComparableUnits().Select((line, i) => Tuple.Create(i, line)).ToList();
            linesRight = new StreamReader(streamRight).ReadAllComparableUnits().Select((line, i) => Tuple.Create(i, line)).ToList();
        }

        public Change[] Compare()
        {
            extractSameLines(false);

            var changedLines = extractSameLines(true);
            var newLines = extractNewLines();

            return changedLines.Concat(newLines).OrderBy(l => l.Item1 + l.Item2).ToArray();
        }

        private IEnumerable<Tuple<int, int, string, string>> extractSameLines(bool compareIndexes)
        {
            var rval = new List<Tuple<int, int, string, string>>();

            for (var iLeft = 0; iLeft < linesLeft.Count; iLeft++)
            {
                var iRight = compareIndexes ? linesRight.Select(t => t.Item1).ToList().IndexOf(linesLeft[iLeft].Item1) :
                    linesRight.Select(t => t.Item2).ToList().IndexOf(linesLeft[iLeft].Item2);

                if (iRight == -1)
                    continue;

                rval.Add(Tuple.Create(linesLeft[iLeft].Item1, linesRight[iRight].Item1, linesLeft[iLeft].Item2, linesRight[iRight].Item2));
                linesLeft.RemoveAt(iLeft--);
                linesRight.RemoveAt(iRight);
            }

            return rval;
        }

        private IEnumerable<Tuple<int, int, string, string>> extractNewLines()
        {
            return linesLeft.Select(l => Tuple.Create(l.Item1, -1, l.Item2, (string)null)).Concat(linesRight.Select(l => Tuple.Create(-1, l.Item1, (string)null, l.Item2)));
        }
    }
}