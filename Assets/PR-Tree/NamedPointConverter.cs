using org.khelekore.prtree;

namespace prtree
{
    public class NamedPointConverter : MBRConverter<NamedPoint>
    {
        public double getMinX(NamedPoint np)
        {
            return (double)np.x;
        }

        public double getMinY(NamedPoint np)
        {
            return (double)np.y;
        }

        public double getMaxX(NamedPoint np)
        {
            return (double)np.x;
        }

        public double getMaxY(NamedPoint np)
        {
            return (double)np.y;
        }
    }
}