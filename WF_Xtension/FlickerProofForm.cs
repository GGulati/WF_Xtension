using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF_Xtension
{
	/// <summary>
	/// Creates a form that flickers significantly less than normal Windows Forms
	/// </summary>
	public class FlickerProofForm : Form
	{
		bool transparency;

		/// <summary>
		/// Creates a form that flickers significantly less than normal Windows Forms
		/// </summary>
		public FlickerProofForm() : this(false)
		{
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		/// <summary>
		/// Creates a form that flickers significantly less than normal Windows Forms
		/// </summary>
		/// <param name="transparency">Allows for complete transparency, but is very expensive in terms of CPU</param>
		public FlickerProofForm(bool transparency) : base()
		{
			this.transparency = transparency;
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				if (transparency)
					cp.ExStyle |= 0x20;  // transparency flag
				return cp;
			}
		}

	}
}