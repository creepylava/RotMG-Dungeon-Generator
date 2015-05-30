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
using System.Collections.Generic;
using System.Linq;
using DungeonGenerator.Dungeon;
using DungeonGenerator.Graph;
using DungeonGenerator.Templates;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public enum GenerationStep {
		Initialize,
		Background,
		TargetGeneration,
		SpecialGeneration,
		BranchGeneration,
		Overlay,
		Finish
	}

	public class Generator {
		readonly Random rand;
		readonly DungeonTemplate template;

		RoomCollision collision;
		Node rootNode;
		List<Node> nodes;

		public GenerationStep Step { get; set; }

		public Generator(int seed, DungeonTemplate template) {
			rand = new Random(seed);
			this.template = template;
		}

		public void Generate(GenerationStep? targetStep = null) {
			Step = GenerationStep.Initialize;
			while (Step != targetStep && Step != GenerationStep.Finish) {
				RunStep();
				Step++;
			}
		}

		public IEnumerable<Room> GetRooms() {
			return nodes.Select(node => node.Content);
		}

		void RunStep() {
			switch (Step) {
				case GenerationStep.Initialize:
					template.SetRandom(rand);
					template.Initialize();
					collision = new RoomCollision();
					nodes = new List<Node>();
					break;
				case GenerationStep.TargetGeneration:
					if (!GenerateTargetPath())
						Step = GenerationStep.Initialize;
					break;
			}
		}

		bool PlaceRoom(Room src, Room target, int connPt) {
			var sep = template.RoomSeparation.Random(rand);
			int x, y;
			switch (connPt) {
				case 0:
				case 2:
					// North & South
					int minX = src.Pos.X + template.CorridorWidth - target.Width;
					int maxX = src.Pos.X + src.Width - template.CorridorWidth;
					x = rand.Next(minX, maxX + 1);

					if (connPt == 0)
						y = src.Pos.Y + src.Height + sep;
					else
						y = src.Pos.Y - sep - target.Height;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return false;
					break;
				case 1:
				case 3:
					// East & West
					int minY = src.Pos.Y + template.CorridorWidth - target.Height;
					int maxY = src.Pos.Y + src.Height - template.CorridorWidth;
					y = rand.Next(minY, maxY + 1);

					if (connPt == 1)
						x = src.Pos.X + src.Width + sep;
					else
						x = src.Pos.X - sep - target.Width;

					target.Pos = new Point(x, y);
					if (collision.HitTest(target))
						return false;
					break;
			}

			collision.Add(target);
			return true;
		}

		int GetMaxConnectionPoints(Room rm) {
			return 4;
		}

		bool GenerateTargetPath() {
			var targetDepth = (int)template.TargetDepth.NextValue();

			var rootRm = template.CreateStart(0);
			rootRm.Pos = new Point(0, 0);
			collision.Add(rootRm);

			rootNode = new Node(rootRm, 0);
			nodes.Add(rootNode);

			return GenerateTargetPathInternal(rootNode, 0, targetDepth);
		}

		bool GenerateTargetPathInternal(Node prev, int depth, int targetDepth) {
			var prevRoom = prev.Content;
			Room rm;
			if (targetDepth == depth)
				rm = template.CreateTarget(depth, prevRoom);
			else
				rm = template.CreateNormal(depth, prevRoom);

			bool targetPlaced;
			do {
				var connPtNum = GetMaxConnectionPoints(prevRoom);
				for (int i = 0; i < connPtNum; i++)
					if (PlaceRoom(prevRoom, rm, i)) {
						connPtNum = -1;
						break;
					}

				if (connPtNum != -1)
					return false;

				var node = new Node(rm, depth);
				Edge.Link(prev, node);
				nodes.Add(node);

				if (targetDepth == depth)
					targetPlaced = true;
				else
					targetPlaced = GenerateTargetPathInternal(node, depth + 1, targetDepth);
			} while (!targetPlaced);

			return true;
		}
	}
}