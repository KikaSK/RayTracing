using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PG2.Mathematics
{
    public struct Vector3
    {
        #region Properties

        public Double X;
        public Double Y;
        public Double Z;

        public Double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vector3 Normalized
        {
            get
            {
                Double ilength = 1.0 / Math.Sqrt(X * X + Y * Y + Z * Z);
                return new Vector3(ilength * X, ilength * Y, ilength * Z);
            }
        }

        public static Vector3 Zero
        {
            get { return new Vector3(0, 0, 0); }
        }

        #endregion


        #region Init

        public Vector3(Double x, Double y, Double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion


        #region Object

        public override String ToString()
        {
            return "(" + X.ToString("F2") + "; " + Y.ToString("F2") + "; " + Z.ToString("F2") + ")";
        }

        #endregion


        #region Arithmetic Operations

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, Double b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator *(Double a, Vector3 b)
        {
            return new Vector3(a * b.X, a * b.Y, a * b.Z);
        }

        // Dot Product
        public static Double operator *(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // 3D Cross Product
        public static Vector3 operator %(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        // Modulate vector a with b
        public static Vector3 operator ^(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static double GetLengthSquared(Vector3 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
        }
        public static Vector3 Normalize(Vector3 v)
        {
            return new Vector3(v.X / Math.Sqrt(GetLengthSquared(v)), v.Y / Math.Sqrt(GetLengthSquared(v)), v.Z / Math.Sqrt(GetLengthSquared(v)));
        }

        public static Vector3 Clamp(Vector3 v, Double min, Double max)
        {
            if (v.X == 0.0 && v.Y == 0.0 && v.Z == 0.0) return v;
            Double minval = MathEx.Min3(v.X, v.Y, v.Z);
            if (minval < min)
            {
                Double delta = min - minval;
                v.X += delta;
                v.Y += delta;
                v.Y += delta;
            }

            Double maxval = MathEx.Max3(v.X, v.Y, v.Z);
            if (maxval > max)
            {
                Double scale = max / maxval;
                v.X *= scale;
                v.Y *= scale;
                v.Z *= scale;
            }

            return new Vector3(v.X, v.Y, v.Z);
        }

        // Calculates reflection vector (normal - unit normal, dir - view vector)
        public static void Reflect(Vector3 normal, Vector3 dir, out Vector3 result)
        {
            // TODO: Calculate reflection vector
            Vector3 n = normal.Normalized;
            Vector3 s = dir.Normalized;
            result = (2*(s * n) * n - s).Normalized;
        }

        // Calculates refraction vector (normal - unit normal, dir - view vector, refIndexRatio = n1/n2 - n1,n2 refraction indices)
        // Procedure must return true if total internal reflection happened, else return false
        public static Boolean Refract(Vector3 normal, Vector3 dir, Double refIndexRatio, out Vector3 result)
        {
            if(refIndexRatio > 1)
            {
                double theta1 = Math.Acos(normal.Normalized * dir.Normalized);
                double criticalangle = Math.Asin(1 / refIndexRatio);
                if (theta1 >= criticalangle)
                {
                    result = Vector3.Zero;
                    return true;
                }
            }
            

            // TODO: Calculate refraction vector, use Snell law
            Vector3 n = normal.Normalized;
            Vector3 s = dir.Normalized;
            double n1n2 = refIndexRatio;
            double dot = n * s;
            Vector3 result1 = -n1n2 * (s - dot * n);
            Vector3 result2 = Math.Sqrt(1 - n1n2 * n1n2 * (1 - (s * n) * (s * n))) * n;
            result = (result1 - result2);
            result = result.Normalized;
            return false;
        }

        #endregion

    }
}
