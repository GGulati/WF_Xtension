using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF_Xtension
{
	/// <summary>
	/// Creates a button that flickers significantly less than normal Windows Forms buttons
	/// </summary>
	public class FlickerProofButton : Button
	{
		/// <summary>
		/// Creates a button that flickers significantly less than normal Windows Forms buttons
		/// </summary>
		public FlickerProofButton() : base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
