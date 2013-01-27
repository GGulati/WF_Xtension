using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF_Xtension
{
	/// <summary>
	/// Creates a list view that flickers significantly less than normal Windows Form list views
	/// </summary>
	public class FlickerProofListView : ListView
	{
		/// <summary>
		/// Creates a list view that flickers significantly less than normal Windows Form list views
		/// </summary>
		public FlickerProofListView() : base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
