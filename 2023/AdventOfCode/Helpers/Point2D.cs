namespace Helpers
{
    public record Point2D(double X, double Y)
    {
        public static Point2D operator +(Point2D a, Point2D b) => new(a.X + b.X, a.Y + b.Y);

        public static Point2D operator -(Point2D a, Point2D b) => new(a.X - b.X, a.Y - b.Y);
    }
}