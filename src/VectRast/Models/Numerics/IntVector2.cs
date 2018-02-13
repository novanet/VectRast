using System;

namespace VectRast.Models.Numerics
{
    public struct IntVector2 : IComparable
    {
        public Int32 x, y;
        public bool defined;
        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x - v2.x, v1.y - v2.y);
        }
        public IntVector2(Int32 x, Int32 y)
        {
            this.x = x;
            this.y = y;
            defined = true;
        }
        public bool extension(IntVector2 otherVector)
        {
            return (x * y == 0 && x * otherVector.y - y * otherVector.x == 0); // parallel to either axis AND colinear
        }
        public override int GetHashCode()
        {
            return x + y * 7919;
        }
        public override bool Equals(object v)
        {
            try
            {
                return x == ((IntVector2)v).x && y == ((IntVector2)v).y;
            }
            catch
            {
                return false;
            }
        }
        int IComparable.CompareTo(object o)
        {
            // lexicographic
            IntVector2 v = (IntVector2)o;
            if (x < v.x || x == v.x && y < v.y)
                return -1;
            if (x > v.x || x == v.x && y > v.y)
                return 1;
            return 0;
        }
    }
}