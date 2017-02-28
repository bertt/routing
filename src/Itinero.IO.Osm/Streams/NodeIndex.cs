// Itinero - Routing for .NET
// Copyright (C) 2017 Abelshausen Ben
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

using Itinero.LocalGeo;
using System;
using System.Collections.Generic;

namespace Itinero.IO.Osm.Streams
{
    /// <summary>
    /// A cache for node coordinates.
    /// </summary>
    public sealed class NodeIndex
    {
        private readonly UnsignedNodeIndex _negativeNodeIndex;
        private readonly UnsignedNodeIndex _postiveNodeIndex;

        private readonly List<long> _extraIds;
        private readonly Dictionary<long, Tuple<int, int, int>> _extraData;

        public NodeIndex()
        {
            _negativeNodeIndex = new UnsignedNodeIndex();
            _postiveNodeIndex = new UnsignedNodeIndex();

            _extraIds = new List<long>();
            _extraData = new Dictionary<long, Tuple<int, int, int>>();
        }

        private bool _isSorted = false;

        /// <summary>
        /// Adds a node id to the index.
        /// </summary>
        public void AddId(long id)
        {
            if (_isSorted)
            {
                _extraIds.Add(id);
                _extraData[id] = new Tuple<int, int, int>(
                    _extraIds.Count - 1,
                    int.MaxValue,
                    int.MaxValue);
                return;
            }

            if (id >= 0)
            {
                _postiveNodeIndex.AddId(id);
            }
            else
            {
                _negativeNodeIndex.AddId(-id);
            }
        }

        /// <summary>
        /// Sorts and converts the index.
        /// </summary>
        public void SortAndConvertIndex()
        {
            _postiveNodeIndex.SortAndConvertIndex();
            _negativeNodeIndex.SortAndConvertIndex();

            _isSorted = true;
        }

        /// <summary>
        /// Gets the node id at the given index.
        /// </summary>
        public long this[long idx]
        {
            get
            {
                if (idx >= _negativeNodeIndex.Count)
                {
                    var postiveIdx = idx - _negativeNodeIndex.Count;
                    if (postiveIdx >= _postiveNodeIndex.Count)
                    {
                        return _extraIds[(int)(postiveIdx - _postiveNodeIndex.Count)];
                    }
                    return _postiveNodeIndex[postiveIdx];
                }
                return _negativeNodeIndex[idx];
            }
        }

        /// <summary>
        /// Sets a vertex id for the given vertex.
        /// </summary>
        public void Set(long id, uint vertex)
        {
            Tuple<int, int, int> extra;
            if (_isSorted && _extraData.TryGetValue(id, out extra))
            {
                _extraData[id] = new Tuple<int, int, int>(
                    extra.Item1,
                    unchecked((int)vertex),
                    int.MinValue);
                return;
            }

            if (id >= 0)
            {
                _postiveNodeIndex.Set(id, vertex);
            }
            else
            {
                _negativeNodeIndex.Set(-id, vertex);
            }
        }

        /// <summary>
        /// Gets the index for the given node in the data array.
        /// </summary>
        public long TryGetIndex(long id)
        {
            Tuple<int, int, int> extra;
            if (_isSorted && _extraData.TryGetValue(id, out extra))
            {
                return _postiveNodeIndex.Count * 2 +
                    _negativeNodeIndex.Count * 2 +
                    extra.Item1 * 2;
            }

            if (id >= 0)
            {
                return _postiveNodeIndex.TryGetIndex(id);
            }
            else
            {
                var result = _negativeNodeIndex.TryGetIndex(-id);
                if (result == long.MaxValue)
                {
                    return long.MaxValue;
                }
                return -(result + 1);
            }
        }

        /// <summary>
        /// Sets the coordinate for the given index.
        /// </summary>
        public void SetIndex(long idx, float latitude, float longitude)
        {
            if (_isSorted && idx > 0)
            {
                var maxIdx = _postiveNodeIndex.Count * 2 +
                   _negativeNodeIndex.Count * 2;
                if (idx >= maxIdx)
                {
                    int lat = (int)(latitude * 10000000);
                    int lon = (int)(longitude * 10000000);

                    var id = _extraIds[(int)((maxIdx - idx) / 2)];
                    Tuple<int, int, int> extra;
                    _extraData.TryGetValue(id, out extra);
                    _extraData[id] = new Tuple<int, int, int>(extra.Item1, lat, lon);
                    return;
                }
            }

            if (idx >= 0)
            {
                _postiveNodeIndex.SetIndex(idx, latitude, longitude);
            }
            else
            {
                idx = -idx - 1;
                _negativeNodeIndex.SetIndex(idx, latitude, longitude);
            }
        }
        /// <summary>
        /// Tries to get a core node and it's matching vertex.
        /// </summary>
        public bool TryGetCoreNode(long id, out uint vertex)
        {
            Tuple<int, int, int> extra;
            if (_isSorted && _extraData.TryGetValue(id, out extra))
            {
                vertex = unchecked((uint)extra.Item2);
                return extra.Item3 == int.MinValue;
            }

            if (id >= 0)
            {
                return _postiveNodeIndex.TryGetCoreNode(id, out vertex);
            }
            else
            {
                return _negativeNodeIndex.TryGetCoreNode(-id, out vertex);
            }
        }

        /// <summary>
        /// Gets the coordinate for the given node.
        /// </summary>
        public bool TryGetValue(long id, out Coordinate coordinate, out bool isCore)
        {
            float latitude, longitude;
            uint vertex;
            if (this.TryGetValue(id, out latitude, out longitude, out isCore, out vertex))
            {
                coordinate = new Coordinate()
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
                return true;
            }
            coordinate = new Coordinate();
            return false;
        }

        /// <summary>
        /// Gets all relevant info on the given node.
        /// </summary>
        public bool TryGetValue(long id, out float latitude, out float longitude, out bool isCore, out uint vertex)
        {
            Tuple<int, int, int> extra;
            if (_isSorted && _extraData.TryGetValue(id, out extra))
            {
                if (extra.Item2 == int.MaxValue &&
                    extra.Item3 == int.MaxValue)
                {
                    latitude = float.MaxValue;
                    longitude = float.MaxValue;
                    vertex = uint.MaxValue;
                    isCore = false;
                    return false;
                }
                if (extra.Item3 == int.MinValue)
                {
                    latitude = float.MaxValue;
                    longitude = float.MaxValue;
                    vertex = unchecked((uint)extra.Item2);
                    isCore = true;
                    return true;
                }
                latitude = (float)(extra.Item2 / 10000000.0);
                longitude = (float)(extra.Item3 / 10000000.0);
                vertex = uint.MaxValue;
                isCore = false;
                return true;
            }

            if (id >= 0)
            {
                return _postiveNodeIndex.TryGetValue(id, out latitude, out longitude, out isCore, out vertex);
            }
            else
            {
                return _negativeNodeIndex.TryGetValue(-id, out latitude, out longitude, out isCore, out vertex);
            }
        }

        /// <summary>
        /// Returns the number of elements.
        /// </summary>
        public long Count
        {
            get
            {
                if (_extraIds != null)
                {
                    return _negativeNodeIndex.Count +
                        _postiveNodeIndex.Count +
                        _extraIds.Count;
                }
                return _negativeNodeIndex.Count +
                    _postiveNodeIndex.Count;
            }
        }
    }
}
