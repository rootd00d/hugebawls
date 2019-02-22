using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HugeBawls
{
    public class Vector3D
    {
        double m_I;
        double m_J;
        double m_K;

        public Vector3D(double x, double y, double z)
        {
            m_I = x;
            m_J = y;
            m_K = z;
        }

        public Vector3D(double s)
        {
            m_I = s;
            m_J = 0;
            m_K = 0;
        }

        public double Magnitude()
        {
            return (double)Math.Sqrt(MagnitudeSq());
        }

        public double MagnitudeSq()
        {
            try
            {
                return (I * I + J * J + K * K);
            }
            catch(OverflowException)
            {
                return double.MaxValue;
            }
        }

        public void Normalize()
        {
            double mag = Magnitude();

            if (mag > Settings.VectorMinMagnitude)
            {
                I /= mag;
                J /= mag;
                K /= mag;
            }
        }

        public Vector3D Normalized()
        {
            double mag = Magnitude();

            if (mag > Settings.VectorMinMagnitude)
            {
                Vector3D normal = new Vector3D(I / mag, J / mag, K / mag);

                return normal;
            }

            return (new Vector3D(I, J, K));
        }

        public double Dot(Vector3D vector)
        {
            return (this.I * vector.I + this.J * vector.J + this.K * vector.K);
        }

        public Vector3D Cross(Vector3D vector)
        {
            return new Vector3D(
              this.J * vector.K - this.K * vector.J,
              this.K * vector.I - this.I * vector.K,
              this.I * vector.J - this.J * vector.I);
        }

        public double Distance(Vector3D vector)
        {
            return (double)Math.Sqrt((double)DistanceSq(vector));
        }

        public double DistanceSq(Vector3D vector)
        {
            double x = (this.I - vector.I) * (this.I - vector.I);
            double y = (this.J - vector.J) * (this.J - vector.J);
            double z = (this.K - vector.K) * (this.K - vector.K);

            return (x + y + z);
        }

        public void Truncate(double maximum)
        {
            if (Magnitude() <= maximum)
            {
                return;
            }

            Normalize();

            I *= maximum;
            J *= maximum;
            K *= maximum;
        }

        public void Rotate2D(double angle)
        {
            double Ip = I;
            double Jp = J;

            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            I = Ip * cos - Jp * sin;
            J = Jp * sin + Ip * cos;
        }

        public Vector3D Rotated2D(double angle)
        {
            Vector3D n = new Vector3D(I, J, K);

            n.Rotate2D(angle);

            return n;
        }


        public double I
        {
            get { return m_I; }
            set { m_I = value; }
        }

        public double J
        {
            get { return m_J; }
            set { m_J = value; }
        }

        public double K
        {
            get { return m_K; }
            set { m_K = value; }
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.I + b.I, a.J + b.J, a.K + b.K);
        }

        public static Vector3D operator +(Vector3D a, double s)
        {
            return a + new Vector3D(s, 0, 0);
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.I - b.I, a.J - b.J, a.K - b.K);
        }

        public static Vector3D operator *(Vector3D a, double s)
        {
            return new Vector3D(a.I * s, a.J * s, a.K * s);
        }

        public static Vector3D operator /(Vector3D a, double s)
        {
            return new Vector3D(a.I / s, a.J / s, a.K / s);
        }

        public static implicit operator Vector3D(double s)
        {
            return new Vector3D(s);
        }

        public static implicit operator Vector3D(int s)
        {
            return new Vector3D((double)s);
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            return (a.I == b.I && a.J == b.J && a.K == b.K);
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            return this == (Vector3D)o;
        }

        public override int GetHashCode()
        {
            return (int)(Magnitude());

        }

        public static bool operator >(Vector3D a, Vector3D b)
        {
            return (a.Magnitude() > b.Magnitude());
        }

        public static bool operator <(Vector3D a, Vector3D b)
        {
            return (a.Magnitude() < b.Magnitude());
        }

        public static bool operator <=(Vector3D a, Vector3D b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >=(Vector3D a, Vector3D b)
        {
            return (a > b) || (a == b);
        }


        public static implicit operator Point(Vector3D a)
        {
            return new Point((int)a.I, (int)a.J);
        }

    }

}