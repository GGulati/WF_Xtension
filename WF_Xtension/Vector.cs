using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WF_Xtension
{
    /// <summarm_y>
    /// Creates a Euclidean geometrical 2-dimensional vector
    /// </summarm_y>
    public struct Vector
    {
        const float PI = (float)Math.PI;//Preform a cast only once for code optimization

        #region Variables
        float m_x, m_y;
        #endregion

        /// <summarm_y>
        /// Creates a Euclidean geometrical 2-dimensional vector
        /// </summarm_y>
        /// <param name="m_x">Magnitude of x aspect of vector</param>
        /// <param name="m_x">Magnitude of y aspect of vector</param>
        public Vector(float x, float y)
        {
            m_x = x;
            m_y = y;
        }

        #region Methods
        /// <summarm_y>
        /// Normalizes the vector to a unit vector
        /// and stores it as the current vector
        /// </summarm_y>
        public void NormalizeInPlace()
        {
            m_x /= Magnitude;
            m_y /= Magnitude;
        }

        /// <summarm_y>
        /// Normalizes the vector to a unit vector
        /// </summarm_y>
        /// <returns>Normalized unit vector</returns>
        public Vector Normalize()
        {
            return new Vector(m_x /= Magnitude, m_y /= Magnitude);
        }

        /// <summary>
        /// Calculates the angle between this vector
        /// and one other vector
        /// </summary>
        /// <param name="other">Vector to use for calculations</param>
        /// <returns>Angle, in degrees, between the two vectors</returns>
        public float AngleBetween(Vector other)
        {
            return (float)Math.Acos(Normalize() * other.Normalize() * 180f / PI);
        }

        /// <summary>
        /// Calculates the reflection vector between this
        /// and another vector
        /// </summary>
        /// <param name="normal">Vector normal to use</param>
        /// <returns>Returns reflected vector</returns>
        public Vector Reflect(Vector normal)
        {
            float mult = 2f * (m_x * normal.m_x + m_y * normal.m_y) / (normal.m_x * normal.m_x + normal.m_y * normal.m_y);
            return new Vector(m_x - normal.m_x * mult, m_y - normal.m_y * mult);
        }

        /// <summary>
        /// Calculates the reflection vector between this
        /// and another vector and stores it in this vector
        /// </summary>
        /// <param name="normal">Vector normal to use</param>
        public void ReflectInPlace(Vector normal)
        {
            float mult = 2f * (m_x * normal.m_x + m_y * normal.m_y) / (normal.m_x * normal.m_x + normal.m_y * normal.m_y);
            m_x -= normal.m_x * mult;
            m_y -= normal.m_y * mult;
        }

        /// <summarm_y>
        /// Returns the X and Y directions of this instance
        /// </summarm_y>
        public override string ToString()
        {
            return "(" + m_x + ", " + m_y + ")";
        }
        #endregion

        #region Accessors
        /// <summarm_y>
        /// Calculates and returns the angle of the vector, in degrees
        /// </summarm_y>
        public float AngleInDegrees { get { return (float)Math.Atan(m_y / m_x) * 180 / PI; } }

        /// <summarm_y>
        /// Calculates and returns the angle of the vector, in radians
        /// </summarm_y>
        public float AngleInRadians { get { return (float)Math.Atan(m_y / m_x); } }

        /// <summarm_y>
        /// Gets or sets m_x aspect of vector
        /// </summarm_y>
        public float X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        /// <summarm_y>
        /// Gets or sets m_y aspect of vector
        /// </summarm_y>
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }
        /// <summarm_y>
        /// Gets the magnitude of the vector
        /// </summarm_y>
        public float Magnitude
        {
            get { return (float)Math.Sqrt(m_x * m_x + m_y * m_y); }
        }
        #endregion

        #region PredefinedVectors
        public static readonly Vector
            Zero = new Vector(0, 0),
            Left = new Vector(-1, 0),
            Right = new Vector(1, 0),
            Up = new Vector(-1, 0),
            Down = new Vector(1, 0);
        #endregion

        #region OperatorOverloading
        public static Vector operator +(Vector lhs, Vector rhs)
        {
            return new Vector(lhs.m_x + rhs.m_x, lhs.m_y + rhs.m_y);
        }
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            return new Vector(lhs.m_x - rhs.m_x, lhs.m_y - rhs.m_y);
        }
        public static Vector operator *(Vector lhs, float rhs)
        {
            return new Vector(lhs.m_x * rhs, lhs.m_y * rhs);
        }
        public static Vector operator *(float lhs, Vector rhs)
        {
            return new Vector(rhs.m_x * lhs, rhs.m_y * lhs);
        }
        public static float operator *(Vector lhs, Vector rhs)
        {
            return lhs.m_x * rhs.m_x + lhs.m_y * rhs.m_y;
        }
        public static Vector operator /(Vector lhs, float rhs)
        {
            return new Vector(lhs.m_x / rhs, lhs.m_y / rhs);
        }

        public static PointF operator +(PointF lhs, Vector rhs)
        {
            return new PointF(lhs.X + rhs.m_x, lhs.Y + rhs.m_y);
        }
        public static PointF operator +(Vector lhs, PointF rhs)
        {
            return new PointF(rhs.X + lhs.m_x, rhs.Y + lhs.m_y);
        }
        public static implicit operator PointF(Vector vec)
        {
            return new PointF(vec.m_x, vec.m_y);
        }
        public static explicit operator Vector(PointF point)
        {
            return new Vector(point.X, point.Y);
        }
        #endregion
    }
}
