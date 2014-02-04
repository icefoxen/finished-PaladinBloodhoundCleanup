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

		static public void Main()
		{
			Application.Run(new MainClass());
		}
	}
}
