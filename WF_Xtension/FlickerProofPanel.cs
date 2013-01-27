using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WF_Xtension
{
	/// <summary>
	/// Creates a panel that is double buffered, and therefore doesn't flicker
	/// </summary>
	public class FlickerProofPanel : Panel
	{
		/// <summary>
		/// Creates a panel that flickers significantly less than normal Windows Form panels
		/// </summary>
		public FlickerProofPanel() : base()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
