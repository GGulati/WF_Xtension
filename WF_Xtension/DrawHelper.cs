using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WF_Xtension
{
	/// <summary>
	/// Assists in drawing on Windows Forms
	/// </summary>
	public static class DrawHelper
	{
		/// <summary>
		/// Draws a image using a source rectangle
		/// </summary>
		/// <param name="draw">Graphics to draw with</param>
		/// <param name="toDraw">Source image to draw with</param>
		/// <param name="x">X position on the Graphics to draw at</param>
		/// <param name="y">Y position on the Graphics to draw at</param>
		/// <param name="sourceRect">Source rectangle of the Image</param>
		public static void DrawImage(Graphics draw, Image toDraw, float x, float y, Rectangle sourceRect)
		{
			draw.DrawImage(toDraw, x, y, sourceRect, GraphicsUnit.Pixel);
		}

		/// <summary>
		/// Draws a image using a source rectangle
		/// </summary>
		/// <param name="draw">Graphics to draw with</param>
		/// <param name="toDraw">Source image to draw with</param>
		/// <param name="targetRect">Rectangle on the Graphics to draw at</param>
		/// <param name="sourceRect">Source rectangle of the Image</param>
		public static void DrawImage(Graphics draw, Image toDraw, Rectangle targetRect, Rectangle sourceRect)
		{
			draw.DrawImage(toDraw, targetRect, sourceRect, GraphicsUnit.Pixel);
		}
	}
}
