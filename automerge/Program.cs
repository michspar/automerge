using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automerge
{
    using System.Dynamic;
    using Change = Tuple<int, int, string, string>;

    class Program
    {
        static Change[] changeset1, changeset2;

        static void Main(string[] args)
        {
            var comparer = new StreamComparer(File.OpenRead(args[0]), File.OpenRead(args[1]));

            changeset1 = comparer.Compare();

            comparer = new StreamComparer(File.OpenRead(args[0]), File.OpenRead(args[2]));

            changeset2 = comparer.Compare();

            var autochanges = changeset1.Except(changeset2, new ConflictChangesComparer()).Union(changeset2.Except(changeset1, new ConflictChangesComparer())).ToArray();
        }
    }
}
