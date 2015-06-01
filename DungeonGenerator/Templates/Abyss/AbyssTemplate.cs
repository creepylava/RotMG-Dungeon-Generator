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

namespace DungeonGenerator.Templates.Abyss {
	public class AbyssTemplate : DungeonTemplate {
		internal static readonly DungeonTile[,] MapTemplate;

		static AbyssTemplate() {
			MapTemplate = ReadTemplate(typeof(AbyssTemplate));
		}

		public override int MaxDepth { get { return 50; } }

		NormDist targetDepth;
		public override NormDist TargetDepth { get { return targetDepth; } }

		public override NormDist SpecialRmCount { get { return null; } }

		public override NormDist SpecialRmDepthDist { get { return null; } }

		public override Range RoomSeparation { get { return new Range(0, 1); } }

		public override int CorridorWidth { get { return 2; } }

		public override void Initialize() {
			targetDepth = new NormDist(5, 25, 15, 40, Rand.Next());
		}

		public override Room CreateStart(int depth) {
			return new StartRoom(16);
		}

		public override Room CreateTarget(int depth, Room prev) {
			return new BossRoom();
		}

		public override Room CreateSpecial(int depth, Room prev) {
			throw new InvalidOperationException();
		}

		public override Room CreateNormal(int depth, Room prev) {
			return new NormalRoom(8, 8);
		}
	}
}