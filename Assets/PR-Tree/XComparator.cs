using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    class XComparator<T> : IComparer<T>
    {
        private MBRConverter<T> converter;

        public XComparator(MBRConverter<T> converter)
        {
            this.converter = converter;
        }

        public int Compare(T t1, T t2)
        {
            double d1 = converter.getMinX(t1);
            double d2 = converter.getMinX(t1);
            if (d1 > d2) return 1;
            if (d1 == d2) return 0;
            else return -1;
        }
    }
}