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

namespace DungeonGenerator.Templates.Abyss {
	internal class Overlay : MapOverlay {
		readonly Random rand;

		public Overlay(Random rand) {
			this.rand = rand;
		}

		byte[,] GenerateHeightMap(int w, int h) {
			float[,] map = new float[w, h];
			int maxR = Math.Min(w, h);
			int r = rand.Next(maxR * 1 / 3, maxR * 2 / 3);
			int r2 = r * r;

			for (int i = 0; i < 200; i++) {
				int cx = rand.Next(w), cy = rand.Next(h);
				float fact = (float)rand.NextDouble() * 3 + 1;

				for (int x = 0; x < w; x++)
					for (int y = 0; y < h; y++) {
						var z = r2 - ((x - cx) * (x - cx) / fact + (y - cy) * (y - cy) * fact);
						if (z < 0)
							continue;
						map[x, y] += z / r2;
					}
			}

			float max = 0;
			float min = float.MaxValue;
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (map[x, y] > max)
						max = map[x, y];
					else if (map[x, y] < min)
						min = map[x, y];
				}

			byte[,] norm = new byte[w, h];
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					float normVal = (map[x, y] - min) / (max - min);
					norm[x, y] = (byte)(normVal * normVal * byte.MaxValue);
				}
			return norm;
		}

		static int Lerp(int a, int b, float val) {
			return a + (int)((b - a) * val);
		}

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var floor = new DungeonObject {
				ObjectType = AbyssTemplate.PartialRedFloor
			};

			const int Sample = 4;

			int w = rasterizer.Width, h = rasterizer.Height;
			var buf = rasterizer.Bitmap;
			var hm = GenerateHeightMap(w / Sample + 2, h / Sample + 2);

			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType == AbyssTemplate.Lava ||
					    buf[x, y].TileType == AbyssTemplate.Space)
						continue;

					if (buf[x, y].Region == "Treasure") {
						buf[x, y].Region = null;
						continue;
					}

					int dx = x / Sample, dy = y / Sample;
					var hx1 = Lerp(hm[dx, dy], hm[dx + 1, dy], (x % Sample) / (float)Sample);
					var hx2 = Lerp(hm[dx, dy + 1], hm[dx + 1, dy + 1], (x % Sample) / (float)Sample);
					var hv = Lerp(hx1, hx2, (y % Sample) / (float)Sample);

					if ((hv / 10) % 2 == 0) {
						buf[x, y].TileType = AbyssTemplate.Lava;
						if (rand.NextDouble() > 0.9 && buf[x, y].Object == null)
							buf[x, y].Object = floor;
					}
				}
		}
	}
}