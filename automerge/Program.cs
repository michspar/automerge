using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automerge
{
    class Program
    {
        static Tuple<int, string, string>[] changeset;

        static void Main(string[] args)
        {
            var comparer = new StreamComparer(File.OpenRead(args[0]), File.OpenRead(args[1]));

            changeset = comparer.Compare();
        }


        public static IEnumerable<T> GenerateCollection<T>(Func<bool> condition, Func<T> generator)
        {
            while (condition())
                yield return generator();
        }
    }
}
