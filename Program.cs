using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace PaladinBloodhoundCleanup
{
	class MainClass : Form
	{
		public MainClass()
		{
			Text = "Simple";
			Size = new Size(250, 200);
			CenterToScreen();

			//var t = new TextFieldParser();
		}

		static public void Main(String[] argv)
		{
			var ws = ParseFile(argv[0]);
			ws.Trim(4000, 7500);
			ws.RemoveDuplicates();
			ws.RemoveGaps();
			Console.WriteLine(ws);
			//Application.Run(new MainClass());
		}
		// This will break horribly if the Bloodhound export data format
		// ever changes, alas.
		// Also, despite having the .xls extension, the Bloodhound data is actually
		// in a space-delimited text file.
		// I think the column width is fixed, but splitting on spaces also works.
		static WorkingSet ParseFile(string fname)
		{
			var ws = new WorkingSet();

			var parser = new TextFieldParser(fname);
			parser.TextFieldType = FieldType.Delimited;
			parser.Delimiters = new string[] { " ", "\t" };
			while(!parser.EndOfData) {
				var currentRow = parser.ReadFields();

				/*
				foreach(var s in currentRow) {
					Console.Write("{0},", s);
				}
				Console.WriteLine();
				*/


				// Skip over rows without enough fields (like blanks)
				if(currentRow.Length < 13) {
					continue;
				}

				// Check to see if the first field is a number.  If not, we skip that row.
				// This mainly skips the header.
				int depth;
				double rop, tg, c1, c2, c3, c4, gamma;
				if(!int.TryParse(currentRow[0], out depth)) {
					continue;
				}

				rop = double.Parse(currentRow[1]);
				tg = double.Parse(currentRow[2]);
				c1 = double.Parse(currentRow[3]);
				c2 = double.Parse(currentRow[4]);
				c3 = double.Parse(currentRow[5]);
				c4 = double.Parse(currentRow[10]);
				gamma = double.Parse(currentRow[12]);


				var m = new Measurement(depth, rop, tg, c1, c2, c3, c4, gamma);

				ws.AddMeasurement(m);
			}

			//Console.WriteLine("'{0}'", parser.Delimiters.Length);

			return ws;
		}
	}
}
