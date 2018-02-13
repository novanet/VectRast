using System;
using VectRast.Models.Numerics;

namespace VectRast.Models
{
    public class VectorPixel
    {
        public IntVector2 fromPnt;
        public IntVector2 toPnt;
        public int dx
        {
            get
            {
                return Math.Abs(toPnt.x - fromPnt.x) + 1;
            }
        }
        public int dy
        {
            get
            {
                return Math.Abs(toPnt.y - fromPnt.y) + 1;
            }
        }
        public int sx
        {
            get
            {
                return Math.Sign(toPnt.x - fromPnt.x);
            }
        }
        public int sy
        {
            get
            {
                return Math.Sign(toPnt.y - fromPnt.y);
            }
        }
        public VectorPixel() { }
        public VectorPixel(IntVector2 fromPnt, IntVector2 toPnt)
        {
            this.fromPnt = fromPnt;
            this.toPnt = toPnt;
        }
        public bool linkVector()
        {
            return dx == 2 && dy == 2;
        }
    }
}