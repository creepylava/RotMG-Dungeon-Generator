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

namespace DungeonGenerator.Templates.PirateCave {
	public class PirateCaveTemplate : DungeonTemplate {
		public static readonly TileType LightSand = new TileType(0x00bd, "Light Sand");
		public static readonly TileType BrownLines = new TileType(0x000c, "Brown Lines");
		public static readonly TileType ShallowWater = new TileType(0x0073, "Shallow Water");
		public static readonly TileType Composite = new TileType(0x00fd, "Composite");
		public static readonly TileType Space = new TileType(0x00fe, "Space");


		public override int MaxDepth { get { return 10; } }

		NormDist targetDepth;
		public override NormDist TargetDepth { get { return targetDepth; } }

		public override NormDist SpecialRmCount { get { return null; } }

		public override NormDist SpecialRmDepthDist { get { return null; } }

		public override Range RoomSeparation { get { return new Range(3, 7); } }

		public override int CorridorWidth { get { return 2; } }

		public override void Initialize() {
			targetDepth = new NormDist(1, 5.5f, 4, 7, Rand.Next());
		}

		public override Room CreateStart(int depth) {
			return new StartRoom(10);
		}

		public override Room CreateTarget(int depth, Room prev) {
			return new BossRoom(10);
		}

		public override Room CreateSpecial(int depth, Room prev) {
			throw new InvalidOperationException();
		}

		public override Room CreateNormal(int depth, Room prev) {
			return new NormalRoom(Rand.Next(8, 15), Rand.Next(8, 15));
		}

		public override MapCorridor CreateCorridor() {
			return new Corridor();
		}

		public override MapBackground CreateBackground() {
			return new Background();
		}

		public override MapOverlay CreateOverlay() {
			return new Overlay(Rand);
		}
	}
}