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

namespace DungeonGenerator.Templates.PirateCave {
	public class NormalRoom : Room {
		readonly int w;
		readonly int h;

		public NormalRoom(int w, int h) {
			this.w = w;
			this.h = h;
		}

		public override RoomType Type { get { return RoomType.Normal; } }

		public override int Width { get { return w; } }

		public override int Height { get { return h; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer) {
			rasterizer.FillRect(Bounds, new DungeonTile {
				TileType = PirateCaveTemplate.BrownLines
			});
		}
	}
}