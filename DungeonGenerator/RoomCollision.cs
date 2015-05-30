/*
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
using DungeonGenerator.Dungeon;
using RotMG.Common;

namespace DungeonGenerator {
	public class RoomCollision {
		const int GridSize = 10;
		readonly SpatialStorage<Room> storage = new SpatialStorage<Room>(new SpatialNodePool<Room>());

		public void Add(Room rm) {
			var bounds = rm.Bounds;
			int x = bounds.X, y = bounds.Y;
			for (; y < bounds.MaxY; y += GridSize) {
				for (x = bounds.X; x < bounds.MaxX; x += GridSize)
					storage.New(rm, x / GridSize, y / GridSize);
				storage.New(rm, x / GridSize, y / GridSize);
			}
			storage.New(rm, x / GridSize, y / GridSize);
		}

		public bool HitTest(Room rm) {
			var bounds = rm.Bounds;
			bool hit = false;
			Action<SpatialNode<Room>> check = node => {
				if (!node.Item.Bounds.Intersection(bounds).IsEmpty)
					hit = true;
			};

			int x = bounds.X, y = bounds.Y;
			for (; y < bounds.MaxY && !hit; y += GridSize) {
				for (x = bounds.X; x < bounds.MaxX && !hit; x += GridSize)
					storage.HitTest(x / GridSize, y / GridSize, check);
				storage.HitTest(x / GridSize, y / GridSize, check);
			}
			storage.HitTest(x / GridSize, y / GridSize, check);
			return hit;
		}
	}
}