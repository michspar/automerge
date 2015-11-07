using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace automerge
{
    using Change = Tuple<int, int, string, string>;

    public class StreamComparer
    {
        private string[] linesLeft, linesRight;
        private Dictionary<string, int> dictLeft, dictRight;

        public StreamComparer(Stream streamLeft, Stream streamRight)
        {
            linesLeft = new StreamReader(streamLeft).ReadAllComparableUnits().ToArray();
            linesRight = new StreamReader(streamRight).ReadAllComparableUnits().ToArray();
            dictLeft = getLinesToIndexesDictionary(linesLeft);
            dictRight = getLinesToIndexesDictionary(linesRight);
        }

        public Change[] Compare()
        {
            var sameLines = linesLeft.Intersect(linesRight);
            var excludeIndexes = sameLines.SelectMany(l => new[] { dictLeft[l], dictRight[l] }).Distinct();
            var includeIndexes = Enumerable.Range(0, Math.Min(linesLeft.Length, linesRight.Length)).Except(excludeIndexes);
            var movedLines = findMovedLines(sameLines);
            var changedLines = findChangedLines(includeIndexes);
            var newLines = findNewLines(sameLines, includeIndexes);

            return movedLines.Concat(changedLines).Concat(newLines).OrderBy(l => l.Item1).ToArray();
        }

        IEnumerable<Change> findNewLines(IEnumerable<string> sameLines, IEnumerable<int> includeIndexes)
        {
            var leftNewLines = linesLeft.Where((l, i) => !includeIndexes.Contains(i));
            var rightNewLines = linesRight.Where((l, i) => !includeIndexes.Contains(i));
            var newLines = leftNewLines.Union(rightNewLines).Where(l => !sameLines.Contains(l));

            return newLines.Select(l => Tuple.Create(dictionaryValueOrDefault(dictLeft, l, -1), dictionaryValueOrDefault(dictRight, l, -1), dictLeft.ContainsKey(l) ? l : null, dictRight.ContainsKey(l) ? l : null));
        }

        IEnumerable<Change> findChangedLines(IEnumerable<int> includeIndexes)
        {
            return includeIndexes.Select(i => Tuple.Create(i, i, linesLeft[i], linesRight[i]));
        }

        IEnumerable<Change> findMovedLines(IEnumerable<string> sameLines)
        {
            return sameLines.Select(l => Tuple.Create(dictLeft[l], dictRight[l], l, l)).Where(l => l.Item1 != l.Item2);
        }

        Dictionary<string, int> getLinesToIndexesDictionary(IEnumerable<string> coll)
        {
            return coll.Select((l, i) => Tuple.Create(l, i)).ToDictionary(l => l.Item1, l => l.Item2);
        }

        TValue dictionaryValueOrDefault<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            TValue rval;

            if (dict.TryGetValue(key, out rval))
                return rval;

            return defaultValue;
        }
    }
}