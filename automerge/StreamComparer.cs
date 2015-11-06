﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace automerge
{
    public class StreamComparer
    {
        private StreamReader streamLeft, streamRight;
        private string left, right;
        int iLine = -1;

        public StreamComparer(Stream streamLeft, Stream streamRight)
        {
            this.streamLeft = new StreamReader(streamLeft);
            this.streamRight = new StreamReader(streamRight);
        }

        bool AdvanceRead()
        {
            if (streamLeft.EndOfStream || streamRight.EndOfStream)
                return false;

            left = streamLeft.ReadComparableUnit();
            right = streamRight.ReadComparableUnit();
            iLine++;

            return true;
        }

        public Tuple<int, string, string>[] Compare()
        {
            var changes = new List<Tuple<int, string, string>>();

            if (streamLeft.EndOfStream && streamRight.EndOfStream)
                return changes.ToArray();

            if (ReadToEnd(changes))
                return changes.ToArray();

            while (AdvanceRead())
            {
                while (!streamLeft.EndOfStream && !streamRight.EndOfStream && left == right)
                    AdvanceRead();

                if (left == right && ReadToEnd(changes))
                    return changes.ToArray();

                changes.Add(Tuple.Create(iLine, left, right));
            }

            return changes.ToArray();
        }

        bool ReadToEnd(List<Tuple<int, string, string>> changes)
        {
            if (streamLeft.EndOfStream)
            {
                changes.AddRange(streamRight.ReadAllComparableUnits().Select(ch => Tuple.Create(++iLine, (string)null, ch)));

                return true;
            }

            if (streamRight.EndOfStream)
            {
                changes.AddRange(streamLeft.ReadAllComparableUnits().Select(ch => Tuple.Create(++iLine, ch, (string)null)));

                return true;
            }

            return false;
        }
    }
}