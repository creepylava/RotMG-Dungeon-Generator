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
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator {
	public enum GenerationStep {
		Initialize,

		TargetGeneration,
		SpecialGeneration,
		BranchGeneration,

		Finish
	}

	public class Generator {
		readonly Random rand;
		readonly DungeonTemplate template;

		RoomCollision collision;
		Node rootNode;
		List<Node> nodes;
		int maxDepth;
		int maxNodeNum;

		public GenerationStep Step { get; set; }

		public Generator(int seed, DungeonTemplate template) {
			rand = new Random(seed);
			this.template = template;
			Step = GenerationStep.Initialize;
		}

		public void Generate(GenerationStep? targetStep = null) {
			while (Step != targetStep && Step != GenerationStep.Finish) {
				RunStep();
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
					rootNode = null;
					nodes = new List<Node>();
					break;

				case GenerationStep.TargetGeneration:
					if (!GenerateTarget()) {
						Step = GenerationStep.Initialize;
						return;
					}
					break;

				case GenerationStep.BranchGeneration:
					GenerateBranches();
					break;
			}
			Step++;
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

		bool GenerateTarget() {
			var targetDepth = (int)template.TargetDepth.NextValue();

			var rootRm = template.CreateStart(0);
			rootRm.Pos = new Point(0, 0);
			collision.Add(rootRm);

			rootNode = new Node(rootRm, 0);
			nodes.Add(rootNode);

			if (GenerateTargetInternal(rootNode, 1, targetDepth)) {
				maxNodeNum = nodes.Count * 3;
				maxDepth = nodes.Count;
				return true;
			}
			return false;
		}

		bool GenerateTargetInternal(Node prev, int depth, int targetDepth) {
			var prevRoom = prev.Content;

			var connPtNum = GetMaxConnectionPoints(prevRoom);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			bool targetPlaced;
			do {
				Room rm;
				if (targetDepth == depth)
					rm = template.CreateTarget(depth, prevRoom);
				else
					rm = template.CreateNormal(depth, prevRoom);

				bool connected = false;
				foreach (var connPt in seq)
					if (PlaceRoom(prevRoom, rm, connPt)) {
						seq.Remove(connPt);
						connected = true;
						break;
					}

				if (!connected)
					return false;

				var node = new Node(rm, depth);
				Edge.Link(prev, node);
				nodes.Add(node);

				if (targetDepth == depth)
					targetPlaced = true;
				else
					targetPlaced = GenerateTargetInternal(node, depth + 1, targetDepth);
			} while (!targetPlaced);

			return true;
		}

		void GenerateBranches() {
			var targetPathNodes = nodes.ToList();
			rand.Shuffle(targetPathNodes);

			foreach (var node in targetPathNodes) {
				GenerateBranchInternal(node, node.Depth + 1, node.Content.Type == RoomType.Target ? template.MaxDepth : maxDepth);
				if (nodes.Count >= maxNodeNum)
					break;
			}
		}

		void GenerateBranchInternal(Node prev, int depth, int maxDepth) {
			if (depth >= maxDepth)
				return;

			if (nodes.Count >= maxNodeNum)
				return;

			var prevRoom = prev.Content;

			var connPtNum = GetMaxConnectionPoints(prevRoom);
			var seq = Enumerable.Range(0, connPtNum).ToList();
			rand.Shuffle(seq);

			int numBranch = rand.Next(8);
			switch (numBranch) {
				case 0:
				case 1:
					numBranch = 1;
					break;
				case 2:
				case 3:
				case 4:
					numBranch = 2;
					break;
				case 5:
				case 6:
					numBranch = 3;
					break;
				case 7:
					numBranch = 4;
					break;
			}
			numBranch -= prev.Edges.Count;
			for (int i = 0; i < numBranch; i++) {
				var rm = template.CreateNormal(depth, prevRoom);

				bool connected = false;
				foreach (var connPt in seq)
					if (PlaceRoom(prevRoom, rm, connPt)) {
						seq.Remove(connPt);
						connected = true;
						break;
					}

				if (!connected)
					return;

				var node = new Node(rm, depth);
				Edge.Link(prev, node);
				nodes.Add(node);

				GenerateBranchInternal(node, depth + 1, maxDepth);
			}
		}

		public DungeonGraph ExportGraph() {
			if (Step != GenerationStep.Finish)
				throw new InvalidOperationException();
			return new DungeonGraph(template, nodes.ToArray());
		}
	}
}