using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WF_Xtension
{
	/// <summary>
	/// A standard game object used for games
	/// </summary>
	public abstract class GameObject
	{
		#region Variables
		protected string m_name;
		protected float m_rotation;
		protected Vector m_pos, m_velocity, m_accel;
		#endregion

		/// <summary>
		/// A standard game object used for games
		/// </summary>
		/// <param name="name">Name of the GameObject's object type name
		/// Ex: LightSource : GameObject { ... }
		/// LightSource sun = new LightSource("Sun");
		/// LightSource torch1 = new LightSource("Torch"), torch2 = new LightSource("Torch");
		/// </param>
		public GameObject(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			m_name = name;
			m_pos = m_velocity = m_accel = Vector.Zero;
		}

		#region Methods
		/// <summary>
		/// Updates the GameObject
		/// </summary>
		/// <param name="updateTime">Time, in milliseconds, since last update</param>
		public void Update(float updateTime)
		{
			m_velocity += m_accel * (float)updateTime;
			m_pos += m_velocity;
		}

		/// <summary>
		/// Draws the GameObject
		/// </summary>
		/// <param name="draw">Graphics to use for drawing</param>
		public void Draw(Graphics draw)
		{
			Matrix rotate = new Matrix();
			rotate.Rotate(m_rotation);

			Matrix oldTransform = draw.Transform;
			draw.Transform = rotate;
			PreformDraw(draw);
			draw.Transform = oldTransform;
		}
		protected virtual void PreformDraw(Graphics draw) { }
		#endregion
	}
}
