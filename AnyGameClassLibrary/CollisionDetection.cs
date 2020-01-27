using System;
using System.Collections.Generic;
using System.Text;

namespace AnyGameClassLibrary
{
    public static class CollisionDetection
    {
        public static bool Hits(Point p1, Point p2, int collisionDetectRange)
        {
            var xSeparation = Math.Abs(p1.x - p2.x);
            var ySeparation = Math.Abs(p1.y - p2.y);
            return (xSeparation < collisionDetectRange) && (ySeparation < collisionDetectRange);
        }
    }
}
