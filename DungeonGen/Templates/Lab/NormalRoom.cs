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
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Lab {
	internal class NormalRoom : FixedRoom {
		struct RoomTemplate {
			public readonly Rect Bounds;
			public readonly Tuple<Direction, int>[] Connections;

			public RoomTemplate(Rect bounds, params Tuple<Direction, int>[] connections) {
				Bounds = bounds;
				Connections = connections;
			}
		}

		static Rect Rect(int x, int y, int w, int h) {
			return new Rect(x, y, x + w, y + h);
		}

		#region Templates

		static readonly RoomTemplate[] roomTemplates = {
			new RoomTemplate(Rect(24, 0, 26, 24),
				Tuple.Create(Direction.North, 11),
				Tuple.Create(Direction.South, 11),
				Tuple.Create(Direction.East, 10),
				Tuple.Create(Direction.West, 10)
				),
			new RoomTemplate(Rect(50, 0, 16, 12),
				Tuple.Create(Direction.East, 1),
				Tuple.Create(Direction.West, 7)
				),
			new RoomTemplate(Rect(66, 0, 25, 12),
				Tuple.Create(Direction.North, 4),
				Tuple.Create(Direction.North, 17),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.South, 17),
				Tuple.Create(Direction.East, 4),
				Tuple.Create(Direction.West, 4)
				),
			new RoomTemplate(Rect(24, 24, 21, 20),
				Tuple.Create(Direction.North, 8),
				Tuple.Create(Direction.South, 9),
				Tuple.Create(Direction.East, 8),
				Tuple.Create(Direction.West, 9)
				),
			new RoomTemplate(Rect(50, 12, 18, 27),
				Tuple.Create(Direction.North, 7),
				Tuple.Create(Direction.South, 7)
				),
			new RoomTemplate(Rect(68, 12, 22, 31),
				Tuple.Create(Direction.North, 4),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.East, 21)
				),
			new RoomTemplate(Rect(0, 50, 40, 22),
				Tuple.Create(Direction.East, 9),
				Tuple.Create(Direction.West, 9)
				),
			new RoomTemplate(Rect(40, 44, 25, 25),
				Tuple.Create(Direction.North, 4),
				Tuple.Create(Direction.North, 17),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.South, 17),
				Tuple.Create(Direction.East, 4),
				Tuple.Create(Direction.East, 17),
				Tuple.Create(Direction.West, 4),
				Tuple.Create(Direction.West, 17)
				),
			new RoomTemplate(Rect(65, 43, 32, 23),
				Tuple.Create(Direction.South, 14),
				Tuple.Create(Direction.East, 6),
				Tuple.Create(Direction.West, 6)
				),
			new RoomTemplate(Rect(0, 72, 24, 24),
				Tuple.Create(Direction.South, 13),
				Tuple.Create(Direction.East, 6)
				),
			new RoomTemplate(Rect(24, 72, 22, 19),
				Tuple.Create(Direction.North, 2),
				Tuple.Create(Direction.South, 5),
				Tuple.Create(Direction.East, 14)
				),
			new RoomTemplate(Rect(46, 69, 42, 50),
				Tuple.Create(Direction.North, 19),
				Tuple.Create(Direction.South, 19)
				),
			new RoomTemplate(Rect(0, 128, 31, 31),
				Tuple.Create(Direction.North, 13),
				Tuple.Create(Direction.South, 13),
				Tuple.Create(Direction.East, 13),
				Tuple.Create(Direction.West, 13)
				),
			new RoomTemplate(Rect(31, 119, 21, 32),
				Tuple.Create(Direction.North, 15),
				Tuple.Create(Direction.East, 26)
				),
			new RoomTemplate(Rect(52, 119, 25, 12),
				Tuple.Create(Direction.North, 4),
				Tuple.Create(Direction.North, 17),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.South, 17),
				Tuple.Create(Direction.East, 4),
				Tuple.Create(Direction.West, 4)
				),
			new RoomTemplate(Rect(52, 119, 25, 12),
				Tuple.Create(Direction.North, 4),
				Tuple.Create(Direction.North, 17),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.South, 17),
				Tuple.Create(Direction.East, 4),
				Tuple.Create(Direction.West, 4)
				),
			new RoomTemplate(Rect(77, 119, 20, 13),
				Tuple.Create(Direction.East, 5),
				Tuple.Create(Direction.West, 5)
				),
			new RoomTemplate(Rect(52, 132, 28, 28),
				Tuple.Create(Direction.North, 3),
				Tuple.Create(Direction.South, 3),
				Tuple.Create(Direction.East, 8)
				),
			new RoomTemplate(Rect(0, 159, 28, 32),
				Tuple.Create(Direction.South, 4),
				Tuple.Create(Direction.West, 3)
				),
			new RoomTemplate(Rect(32, 152, 32, 21),
				Tuple.Create(Direction.North, 14),
				Tuple.Create(Direction.South, 14)
				),
			new RoomTemplate(Rect(31, 173, 23, 24),
				Tuple.Create(Direction.East, 10),
				Tuple.Create(Direction.West, 10)
				),
			new RoomTemplate(Rect(65, 152, 21, 29),
				Tuple.Create(Direction.North, 8),
				Tuple.Create(Direction.South, 8)
				)
		};

		#endregion

		readonly int currentId;
		RoomTemplate current;

		public NormalRoom(NormalRoom prev, Random rand) {
			do {
				currentId = rand.Next(roomTemplates.Length);
			} while (prev != null && currentId == prev.currentId);
			current = roomTemplates[currentId];
		}

		public override RoomType Type { get { return RoomType.Normal; } }

		public override int Width { get { return current.Bounds.MaxX - current.Bounds.X; } }

		public override int Height { get { return current.Bounds.MaxY - current.Bounds.Y; } }

		public override Tuple<Direction, int>[] ConnectionPoints { get { return current.Connections; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var buf = rasterizer.Bitmap;
			var bounds = Bounds;

			rasterizer.Copy(LabTemplate.MapTemplate, current.Bounds, Pos);
		}
	}
}