using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF_Xtension
{
	public class FlickerProofTabPage : TabPage
	{
		public FlickerProofTabPage() : base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
