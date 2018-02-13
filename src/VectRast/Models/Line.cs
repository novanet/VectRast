using System;
using VectRast.Models.Numerics;

namespace VectRast.Models
{
    public class Line : VectorPixel
    {
        int nextmindx;
        int nextmaxdx;
        int nextmindy;
        int nextmaxdy;
        double maxleft;
        double minright;
        public IntVector2 outerFromPnt;
        public Line(VectorPixel v)
        {
            maxleft = Double.MinValue;
            minright = Double.MaxValue;
            outerFromPnt = v.fromPnt;
            toPnt = v.toPnt;
            nextmindx = 0;
            nextmaxdx = Int32.MaxValue;
            nextmindy = 0;
            nextmaxdy = Int32.MaxValue;
        }
        public bool satisfiesOuter(VectorPixel v)
        {
            return
                Math.Abs(v.toPnt.x - toPnt.x) <= nextmaxdx &&
                Math.Abs(v.toPnt.y - toPnt.y) <= nextmaxdy; // outer segment must be shorter than inner segment
        }
        public bool satisfiesInner(VectorPixel v)
        {
            if (!fromPnt.defined)
                return Math.Abs(toPnt.x - outerFromPnt.x) == Math.Abs(toPnt.y - outerFromPnt.y) ?
                    true : // anything goes
                    Math.Abs(v.toPnt.x - toPnt.x) >= Math.Abs(v.fromPnt.x - outerFromPnt.x) &&
                    Math.Abs(v.toPnt.y - toPnt.y) >= Math.Abs(v.fromPnt.y - outerFromPnt.y); // inner segment must be >= the outer segment
            return
                nextmindx <= Math.Abs(v.toPnt.x - toPnt.x) && Math.Abs(v.toPnt.x - toPnt.x) <= nextmaxdx &&
                nextmindy <= Math.Abs(v.toPnt.y - toPnt.y) && Math.Abs(v.toPnt.y - toPnt.y) <= nextmaxdy;
        }
        public bool sameDir(VectorPixel v)
        {
            if (v.linkVector())
                return
                    Math.Abs(Math.Sign(toPnt.x - outerFromPnt.x) - (v.toPnt.x - toPnt.x)) <= 1 &&
                    Math.Abs(Math.Sign(toPnt.y - outerFromPnt.y) - (v.toPnt.y - toPnt.y)) <= 1;
            if (Math.Abs(toPnt.x - outerFromPnt.x) > Math.Abs(toPnt.y - outerFromPnt.y))
                return
                    Math.Sign(toPnt.x - outerFromPnt.x) == Math.Sign(v.toPnt.x - toPnt.x) &&
                    Math.Abs(Math.Sign(toPnt.y - outerFromPnt.y) - (v.toPnt.y - toPnt.y)) <= 1;
            if (Math.Abs(toPnt.x - outerFromPnt.x) < Math.Abs(toPnt.y - outerFromPnt.y))
                return
                    Math.Abs(Math.Sign(toPnt.x - outerFromPnt.x) - (v.toPnt.x - toPnt.x)) <= 1 &&
                    Math.Sign(toPnt.y - outerFromPnt.y) == Math.Sign(v.toPnt.y - toPnt.y);
            return
                Math.Abs(Math.Sign(toPnt.x - outerFromPnt.x) - Math.Sign(v.toPnt.x - toPnt.x)) <= 1 &&
                Math.Abs(Math.Sign(toPnt.y - outerFromPnt.y) - Math.Sign(v.toPnt.y - toPnt.y)) <= 1;
        }
        public void update(VectorPixel v)
        {
            if (!fromPnt.defined)
                fromPnt = v.fromPnt;
            toPnt = v.toPnt;
            int d1;
            int d2;
            if (dx > dy)
            {
                d1 = dx;
                d2 = dy;
            }
            else
            {
                d1 = dy;
                d2 = dx;
            }
            maxleft = Math.Max(maxleft, 1.0 * (d1 - 1) / d2);
            minright = Math.Min(minright, 1.0 * (d1 + 1) / d2);
            double dmin = maxleft * (d2 + 1) + 0.5 - d1;
            double dmax = minright * (d2 + 1) - 0.5 - d1;
            if (Math.Ceiling(dmin) - dmin == 0.5)
                dmin += 0.5;
            if (Math.Ceiling(dmax) - dmax == 0.5)
                dmax -= 0.5;
            if (dx > dy)
            {
                nextmindx = (int)Math.Round(dmin);
                nextmaxdx = (int)Math.Round(dmax);
                nextmindy = 0;
                nextmaxdy = 1;
            }
            else
                if (dx < dy)
            {
                nextmindx = 0;
                nextmaxdx = 1;
                nextmindy = (int)Math.Round(dmin);
                nextmaxdy = (int)Math.Round(dmax);
            }
            else
            {
                nextmindx = 0;
                nextmaxdx = 2;
                nextmindy = 0;
                nextmaxdy = 2;
            }
        }
    }
}