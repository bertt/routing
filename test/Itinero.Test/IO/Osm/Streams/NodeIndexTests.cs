// Itinero - Routing for .NET
// Copyright (C) 2015 Abelshausen Ben
// 
// This file is part of Itinero.
// 
// Itinero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// Itinero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Itinero. If not, see <http://www.gnu.org/licenses/>.

using Itinero.IO.Osm.Streams;
using NUnit.Framework;

namespace Itinero.Test.IO.Osm.Streams
{
    /// <summary>
    /// Contains tests for the node index.
    /// </summary>
    [TestFixture]
    public class NodeIndexTests
    {
        /// <summary>
        /// Tests negative id indexing.
        /// </summary>
        [Test]
        public void TestNegativeIds()
        {
            var index = new NodeIndex();

            index.AddId(-10000);
            index.AddId(-1000);
            index.AddId(-100);
            index.AddId(100000000);
            index.AddId(-10);
            index.AddId(-10000000);
            index.AddId(10);
            index.AddId(1000000);
            index.AddId(1000);
            index.AddId(-1000000);
            index.AddId(10000);
            index.AddId(100000);
            index.AddId(100);
            index.AddId(-100000);
            index.AddId(10000000);

            index.SortAndConvertIndex();

            index.SetIndex(index.TryGetIndex(-10000000), 1, 2);
            index.SetIndex(index.TryGetIndex(-1000000), 3, 4);
            index.SetIndex(index.TryGetIndex(-100000), 5, 6);
            index.SetIndex(index.TryGetIndex(-10000), 7, 8);
            index.SetIndex(index.TryGetIndex(-1000), 9, 10);
            index.SetIndex(index.TryGetIndex(-100), 11, 12);
            index.SetIndex(index.TryGetIndex(-10), 13, 14);
            index.SetIndex(index.TryGetIndex(10), 15, 16);
            index.SetIndex(index.TryGetIndex(100), 17, 18);
            index.SetIndex(index.TryGetIndex(1000), 19, 20);
            index.SetIndex(index.TryGetIndex(10000), 21, 22);
            index.SetIndex(index.TryGetIndex(100000), 23, 24);
            index.SetIndex(index.TryGetIndex(1000000), 25, 26);
            index.SetIndex(index.TryGetIndex(10000000), 27, 28);

            bool isCore;
            float latitude, longitude;
            uint vertex;
            Assert.IsTrue(index.TryGetValue(-10000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(1, latitude);
            Assert.AreEqual(2, longitude);
            Assert.IsTrue(index.TryGetValue(-1000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(3, latitude);
            Assert.AreEqual(4, longitude);
            Assert.IsTrue(index.TryGetValue(-100000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(5, latitude);
            Assert.AreEqual(6, longitude);
            Assert.IsTrue(index.TryGetValue(-10000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(7, latitude);
            Assert.AreEqual(8, longitude);
            Assert.IsTrue(index.TryGetValue(-1000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(9, latitude);
            Assert.AreEqual(10, longitude);
            Assert.IsTrue(index.TryGetValue(-100, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(11, latitude);
            Assert.AreEqual(12, longitude);
            Assert.IsTrue(index.TryGetValue(-10, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(13, latitude);
            Assert.AreEqual(14, longitude);
            Assert.IsTrue(index.TryGetValue(10, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(15, latitude);
            Assert.AreEqual(16, longitude);
            Assert.IsTrue(index.TryGetValue(100, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(17, latitude);
            Assert.AreEqual(18, longitude);
            Assert.IsTrue(index.TryGetValue(1000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(19, latitude);
            Assert.AreEqual(20, longitude);
            Assert.IsTrue(index.TryGetValue(10000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(21, latitude);
            Assert.AreEqual(22, longitude);
            Assert.IsTrue(index.TryGetValue(100000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(23, latitude);
            Assert.AreEqual(24, longitude);
            Assert.IsTrue(index.TryGetValue(1000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(25, latitude);
            Assert.AreEqual(26, longitude);
            Assert.IsTrue(index.TryGetValue(10000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(27, latitude);
            Assert.AreEqual(28, longitude);
        }

        /// <summary>
        /// Tests adding new data after sort.
        /// </summary>
        [Test]
        public void TestAddAfterSort()
        {
            var index = new NodeIndex();

            index.AddId(-10000);
            index.AddId(-1000);
            index.AddId(-100);
            index.AddId(100000000);
            index.AddId(-10);
            index.AddId(-10000000);
            index.AddId(10);
            index.AddId(1000000);
            index.AddId(1000);
            index.AddId(-1000000);
            index.AddId(10000);
            index.AddId(100000);
            index.AddId(100);
            index.AddId(-100000);
            index.AddId(10000000);

            index.SortAndConvertIndex();

            index.SetIndex(index.TryGetIndex(-10000000), 1, 2);
            index.SetIndex(index.TryGetIndex(-1000000), 3, 4);
            index.AddId(-1234);
            index.Set(-1234, 43210);
            index.SetIndex(index.TryGetIndex(-100000), 5, 6);
            index.SetIndex(index.TryGetIndex(-10000), 7, 8);
            index.SetIndex(index.TryGetIndex(-1000), 9, 10);
            index.SetIndex(index.TryGetIndex(-100), 11, 12);
            index.AddId(123);
            index.Set(123, 321);
            index.SetIndex(index.TryGetIndex(-10), 13, 14);
            index.SetIndex(index.TryGetIndex(10), 15, 16);
            index.SetIndex(index.TryGetIndex(100), 17, 18);
            index.SetIndex(index.TryGetIndex(1000), 19, 20);
            index.SetIndex(index.TryGetIndex(10000), 21, 22);
            index.AddId(-123);
            index.Set(-123, 3210);
            index.SetIndex(index.TryGetIndex(100000), 23, 24);
            index.SetIndex(index.TryGetIndex(1000000), 25, 26);
            index.SetIndex(index.TryGetIndex(10000000), 27, 28);
            
            bool isCore;
            float latitude, longitude;
            uint vertex;
            Assert.IsTrue(index.TryGetValue(-10000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(1, latitude);
            Assert.AreEqual(2, longitude);
            Assert.IsTrue(index.TryGetValue(-1000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(3, latitude);
            Assert.AreEqual(4, longitude);
            Assert.IsTrue(index.TryGetValue(-100000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(5, latitude);
            Assert.AreEqual(6, longitude);
            Assert.IsTrue(index.TryGetValue(-10000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(7, latitude);
            Assert.AreEqual(8, longitude);
            Assert.IsTrue(index.TryGetValue(-1000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(9, latitude);
            Assert.AreEqual(10, longitude);
            Assert.IsTrue(index.TryGetValue(-100, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(11, latitude);
            Assert.AreEqual(12, longitude);
            Assert.IsTrue(index.TryGetValue(-10, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(13, latitude);
            Assert.AreEqual(14, longitude);
            Assert.IsTrue(index.TryGetValue(10, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(15, latitude);
            Assert.AreEqual(16, longitude);
            Assert.IsTrue(index.TryGetValue(100, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(17, latitude);
            Assert.AreEqual(18, longitude);
            Assert.IsTrue(index.TryGetValue(1000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(19, latitude);
            Assert.AreEqual(20, longitude);
            Assert.IsTrue(index.TryGetValue(10000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(21, latitude);
            Assert.AreEqual(22, longitude);
            Assert.IsTrue(index.TryGetValue(100000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(23, latitude);
            Assert.AreEqual(24, longitude);
            Assert.IsTrue(index.TryGetValue(1000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(25, latitude);
            Assert.AreEqual(26, longitude);
            Assert.IsTrue(index.TryGetValue(10000000, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(uint.MaxValue, vertex);
            Assert.IsFalse(isCore);
            Assert.AreEqual(27, latitude);
            Assert.AreEqual(28, longitude);

            Assert.IsTrue(index.TryGetValue(-1234, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(43210, vertex);
            Assert.IsTrue(isCore);
            Assert.AreEqual(float.MaxValue, latitude);
            Assert.AreEqual(float.MaxValue, longitude);
            Assert.IsTrue(index.TryGetValue(-123, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(3210, vertex);
            Assert.IsTrue(isCore);
            Assert.AreEqual(float.MaxValue, latitude);
            Assert.AreEqual(float.MaxValue, longitude);
            Assert.IsTrue(index.TryGetValue(123, out latitude, out longitude, out isCore, out vertex));
            Assert.AreEqual(321, vertex);
            Assert.IsTrue(isCore);
            Assert.AreEqual(float.MaxValue, latitude);
            Assert.AreEqual(float.MaxValue, longitude);
        }

        /// <summary>
        /// Tests the overflow after sorting.
        /// </summary>
        [Test]
        public void TestOverflow()
        {
            var index = new NodeIndex();

            index.AddId(int.MaxValue / 4L);
            index.AddId(int.MaxValue / 2L);
            index.AddId(int.MaxValue);
            index.AddId(int.MaxValue * 2L);
            index.AddId(int.MaxValue * 4L);

            index.SortAndConvertIndex();

            Assert.AreEqual(int.MaxValue / 4L, index[0]);
            Assert.AreEqual(int.MaxValue / 2L, index[1]);
            Assert.AreEqual(int.MaxValue, index[2]);
            Assert.AreEqual(int.MaxValue * 2L, index[3]);
            Assert.AreEqual(int.MaxValue * 4L, index[4]);
        }
    }
}