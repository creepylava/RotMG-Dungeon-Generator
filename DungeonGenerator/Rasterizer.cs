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
using RotMG.Common.BMap;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public enum RasterizationStep {
		Initialize = 5,

		Background = 6,
		Corridor = 7,
		Room = 8,
		Overlay = 9,

		Finish = 10
	}

	internal class Rasterizer {
		readonly Random rand;
		readonly DungeonGraph graph;
		readonly BitmapRasterizer<MapTile> rasterizer;

		const uint Space = 0xfe;

		public RasterizationStep Step { get; set; }

		public Rasterizer(int seed, DungeonGraph graph) {
			rand = new Random(seed);
			this.graph = graph;
			rasterizer = new BitmapRasterizer<MapTile>(graph.Width, graph.Height);
			Step = RasterizationStep.Initialize;
		}

		public void Rasterize(RasterizationStep? targetStep = null) {
			while (Step != targetStep && Step != RasterizationStep.Finish) {
				RunStep();
			}
		}

		void RunStep() {
			switch (Step) {
				case RasterizationStep.Initialize:
					rasterizer.Clear(new MapTile {
						TileType = Space
					});
					graph.Template.InitializeRasterization(graph);
					break;

				case RasterizationStep.Background:
					graph.Template.CreateBackground().Rasterize(rasterizer);
					break;

				case RasterizationStep.Overlay:
					graph.Template.CreateOverlay().Rasterize(rasterizer);
					break;
			}
			Step++;
		}

		public MapTile[,] ExportMap() {
			return rasterizer.Bitmap;
		}
	}
}