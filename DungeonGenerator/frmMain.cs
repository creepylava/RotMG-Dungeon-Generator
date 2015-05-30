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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DungeonGenerator.Dungeon;
using DungeonGenerator.Templates.PirateCave;

namespace DungeonGenerator {
	public partial class frmMain : Form {
		public frmMain() {
			InitializeComponent();
		}

		readonly Random rand = new Random();
		readonly List<Button> btns = new List<Button>();
		Generator gen;

		void frmMain_Load(object sender, EventArgs e) {
			foreach (var value in Enum.GetValues(typeof(GenerationStep))) {
				if ((GenerationStep)value == GenerationStep.Finish)
					continue;
				var btn = new Button { Text = value.ToString(), Tag = value, AutoSize = true };
				btn.Click += Step_Click;
				stepsPane.Controls.Add(btn);
				btns.Add(btn);
			}
			stepsPane.Enabled = false;
		}

		void Step_Click(object sender, EventArgs e) {
			var step = (GenerationStep)((Button)sender).Tag;
			gen.Generate(step + 1);
			Render();
			foreach (var btn in btns)
				btn.Enabled = (GenerationStep)btn.Tag >= gen.Step;
		}

		void Render() {
			var rms = gen.GetRooms().ToList();
			int dx = int.MaxValue, dy = int.MaxValue;
			int mx = int.MinValue, my = int.MinValue;
			foreach (var rm in rms) {
				var bounds = rm.Bounds;

				if (bounds.X < dx)
					dx = bounds.X;
				if (bounds.Y < dy)
					dy = bounds.Y;

				if (bounds.MaxX > mx)
					mx = bounds.MaxX;
				if (bounds.MaxY > my)
					my = bounds.MaxY;
			}

			const int Factor = 4;

			var pen = new Pen(Color.Black, Factor / 2);
			var bmp = new Bitmap((mx - dx + 4) * Factor, (my - dy + 4) * Factor);
			using (var g = Graphics.FromImage(bmp))
				foreach (var rm in rms) {
					var rmPen = pen;
					if (rm.Type == RoomType.Start)
						rmPen = new Pen(Color.Red, Factor / 2);
					else if (rm.Type == RoomType.Target)
						rmPen = new Pen(Color.Green, Factor / 2);
					else if (rm.Type == RoomType.Special)
						rmPen = new Pen(Color.Blue, Factor / 2);

					g.DrawRectangle(rmPen,
						(rm.Pos.X - dx) * Factor + 2 * Factor, (rm.Pos.Y - dy) * Factor + 2 * Factor,
						rm.Width * Factor, rm.Height * Factor);

					if (rmPen != pen)
						rmPen.Dispose();
				}

			var original = box.Image;
			box.Image = bmp;
			if (original != null)
				original.Dispose();
		}

		void btnNew_Click(object sender, EventArgs e) {
			var seed = rand.Next();
			gen = new Generator(seed, new PirateCaveTemplate());
			lblSeed.Text = "Seed: " + seed;

			stepsPane.Enabled = true;
			foreach (var btn in btns)
				btn.Enabled = true;

			var original = box.Image;
			box.Image = null;
			if (original != null)
				original.Dispose();
		}
	}
}