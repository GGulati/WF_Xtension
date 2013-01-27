using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WF_Xtension
{
	/// <summary>
	/// Creates a PictureBox that is double buffered, and therefore doesn't flicker
	/// </summary>
	public class FlickerProofPictureBox : PictureBox
	{
		/// <summary>
		/// Creates a PictureBox that flickers significantly less than normal Windows Form panels
		/// </summary>
		public FlickerProofPictureBox() : base()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
