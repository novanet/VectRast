namespace VectRast.Models.Elma
{
    public class Apple : ElmaObject
    {
        public Apple(double x, double y, uint appleGravity, uint appleAnimationNumber)
            : base(x, y, ElmaObjectTypes.Food, appleGravity, appleAnimationNumber)
        {
        }
    }
}