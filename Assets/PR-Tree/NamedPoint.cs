using System;

namespace prtree
{
    public class NamedPoint
    {
        public readonly double x;
        public readonly double y;
        public readonly int id;

        public NamedPoint(int id, double x, double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return id.ToString();
        }
    }
}
