namespace VectRast.Models.Elma
{
    public class Player : ElmaObject
    {
        public Player(double x, double y)
            : base(x, y, ElmaObjectTypes.Start, 0, 0)
        {
        }
    }
}