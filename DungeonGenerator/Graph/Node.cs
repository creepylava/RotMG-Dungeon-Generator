﻿/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using DungeonGenerator.Dungeon;

namespace DungeonGenerator.Graph {
	internal class Node {
		public Node(Room rm, int depth) {
			Content = rm;
			Depth = depth;
		}

		public Room Content { get; private set; }
		public int Depth { get; private set; }
		public IList<Edge> Edges { get; private set; }

		public Node() {
			Edges = new List<Edge>(4);
		}

		public IEnumerable<Node> GetNeighbors() {
			foreach (var edge in Edges) {
				if (edge.NodeA == this)
					yield return edge.NodeB;
				else
					yield return edge.NodeA;
			}
		}
	}
}