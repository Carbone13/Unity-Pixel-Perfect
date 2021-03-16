using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    class YNodeComparator<T> : IComparer<Node<T>>
    {
        private MBRConverter<T> converter;

        public YNodeComparator(MBRConverter<T> converter)
        {
            this.converter = converter;
        }

        public int Compare(Node<T> n1, Node<T> n2)
        {
            double d1 = n1.getMBR(converter).getMinY();
            double d2 = n2.getMBR(converter).getMinY();
            if (d1 > d2) return 1;
            if (d1 == d2) return 0;
            else return -1;
        }
    }
}
