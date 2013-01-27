using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF_Xtension
{
	/// <summary>
	/// Provides extension methods for math-related methods
	/// </summary>
	public static class MathHelper
	{
		/// <summary>
		/// Clamps a given value to a minimum and maximum
		/// </summary>
		/// <param name="value">Value to clamp</param>
		/// <param name="min">Minimum value the argument can have</param>
		/// <param name="max">Maximum value the argument can have</param>
		/// <returns>Clamped value</returns>
		public static float Clamp(this float value, float min, float max)
		{
            return Math.Min(Math.Max(value, min), max);
		}

        /// <summary>
        /// Clamps a given value to a minimum and maximum
        /// </summary>
        /// <param name="value">Value to clamp</param>
        /// <param name="min">Minimum value the argument can have</param>
        /// <param name="max">Maximum value the argument can have</param>
        /// <returns>Clamped value</returns>
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
	}
}
