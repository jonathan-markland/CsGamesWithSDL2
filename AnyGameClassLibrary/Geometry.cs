using System;

namespace AnyGameClassLibrary
{
    // TODO: These could be made more convenient for construction, and offer functions, eg: Point-Rectangle intersect, rectangle-rectangle intersect.

    public struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point ShuntedBy(int dx, int dy)
        {
            return new Point(x + dx, y + dy);
        }
    }

    public struct Dimensions
    {
        public int Width;
        public int Height;
    }

    public struct Rectangle
    {
        public int Left;
        public int Top;
        public int Width;
        public int Height;
    }

}
