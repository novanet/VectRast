using System;

namespace VectRast.Models.Numerics
{
    public struct DoubleVector2
    {
        public double x, y;
        public DoubleVector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public DoubleVector2(IntVector2 v)
        {
            this.x = (double)v.x;
            this.y = (double)v.y;
        }
        public double length
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }
        public DoubleVector2 perpendicular
        {
            get
            {
                return new DoubleVector2(y, -x);
            }
        }
        public void normalize()
        {
            // throws exception for zero length
            double l = length;
            x /= l;
            y /= l;
        }
        public double scalarProduct(DoubleVector2 dv2)
        {
            return (x * dv2.x + y * dv2.y);
        }
        public static DoubleVector2 operator -(DoubleVector2 v1, DoubleVector2 v2)
        {
            return new DoubleVector2(v1.x - v2.x, v1.y - v2.y);
        }
    }
}