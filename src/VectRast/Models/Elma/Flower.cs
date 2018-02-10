namespace VectRast.Models.Elma
{
    public class Flower : ElmaObject
    {
        public Flower(double x, double y)
            : base(x, y, ElmaObjectTypes.Flower, 0, 0)
        {
        }
    }
}