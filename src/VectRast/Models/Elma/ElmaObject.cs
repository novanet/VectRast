namespace VectRast.Models.Elma
{
    public class ElmaObject
    {
        public double x;
        public double y;
        public ElmaObjectTypes type;
        public uint appleGravity;
        public uint appleAnimationNumber;
        public ElmaObject(double x, double y, ElmaObjectTypes type, uint appleGravity, uint appleAnimationNumber)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.appleGravity = appleGravity;
            this.appleAnimationNumber = appleAnimationNumber;
        }
    }
}