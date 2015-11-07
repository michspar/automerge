using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using automerge;
using System.IO;
using System.Text;
using System.Linq;

namespace automergetest
{
    [TestClass]
    public class CompareTest
    {
        [TestMethod]
        public void TestEqual()
        {
            var left = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));
            var right = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));


            var comparer = new StreamComparer(left, right);

            Assert.IsTrue(comparer.Compare().Length == 0);
        }

        [TestMethod]
        public void TestNotEqualSameLen()
        {
            var left = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));
            var right = new MemoryStream(Encoding.Default.GetBytes(@"line1
line4
line3"));


            var comparer = new StreamComparer(left, right);

            Assert.AreEqual(Tuple.Create(1, "line2", "line4"), comparer.Compare()[0]);
        }

        [TestMethod]
        public void TestEqualDifferentLen()
        {
            var left = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));
            var right = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3
line4"));


            var comparer = new StreamComparer(left, right);

            Assert.AreEqual(Tuple.Create(3, (string)null, "line4"), comparer.Compare()[0]);
        }

        [TestMethod]
        public void TestEqualAddLineToBegin()
        {
            var left = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));
            var right = new MemoryStream(Encoding.Default.GetBytes(@"line4
line1
line2
line3"));


            var comparer = new StreamComparer(left, right);
            var changeset = comparer.Compare();

            Assert.AreEqual(Tuple.Create(0, (string)null, "line4"), changeset[0]);
        }

        [TestMethod]
        public void TestNotEqualDifferentLen()
        {
            var left = new MemoryStream(Encoding.Default.GetBytes(@"line1
line2
line3"));
            var right = new MemoryStream(Encoding.Default.GetBytes(@"line1
line4
line3
line2"));


            var comparer = new StreamComparer(left, right);

            Assert.IsTrue(new[]
            {
                Tuple.Create(1, "line2", "line4"),
                Tuple.Create(3, (string)null, "line2")
            }.SequenceEqual(comparer.Compare()));
        }
    }
}
