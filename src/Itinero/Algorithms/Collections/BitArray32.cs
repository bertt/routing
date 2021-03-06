﻿// Itinero - Routing for .NET
// Copyright (C) 2016 Abelshausen Ben
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

namespace Itinero.Algorithms.Collections
{
    /// <summary>
    /// Represents a large bit array.
    /// </summary>
    public class BitArray32
    {
        private readonly uint[] _array; // Holds the bit vector array.
        private long _length; // Holds the length of this array.

        /// <summary>
        /// Creates a new bitvector array.
        /// </summary>
        public BitArray32(long size)
        {
            _length = size;
            _array = new uint[(int)System.Math.Ceiling((double)size / 32)];
        }

        /// <summary>
        /// Returns the element at the given index.
        /// </summary>
        public bool this[long idx]
        {
            get
            {
                var arrayIdx = (int)(idx >> 5);
                var bitIdx = (int)(idx % 32);
                var mask = (long)1 << bitIdx;
                return (_array[arrayIdx] & mask) != 0;
            }
            set
            {
                var arrayIdx = (int)(idx >> 5);
                var bitIdx = (int)(idx % 32);
                var mask = (long)1 << bitIdx;
                if (value)
                { // set value.
                    _array[arrayIdx] = (uint)(mask | _array[arrayIdx]);
                }
                else
                { // unset value.
                    _array[arrayIdx] = (uint)((~mask) & _array[arrayIdx]);
                }
            }
        }

        /// <summary>
        /// Returns the length of this array.
        /// </summary>
        public long Length
        {
            get
            {
                return _length;
            }
        }
    }
}