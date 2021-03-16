using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    class XNodeComparator<T> : IComparer<Node<T>>
    {
        private MBRConverter<T> converter;

        public XNodeComparator(MBRConverter<T> converter)
        {
            this.converter = converter;
        }

        public int Compare(Node<T> n1, Node<T> n2)
        {
            double d1 = n1.getMBR(converter).getMinX();
            double d2 = n2.getMBR(converter).getMinX();
            //return double.compare(d1, d2);
            if (d1 > d2) return 1;
            if (d1 == d2) return 0;
            else return -1;
        }
    }
}
