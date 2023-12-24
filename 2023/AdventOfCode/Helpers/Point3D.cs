namespace Helpers
{
    public record Point3D(long X, long Y, long Z)
    {
        public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Point3D operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
}
