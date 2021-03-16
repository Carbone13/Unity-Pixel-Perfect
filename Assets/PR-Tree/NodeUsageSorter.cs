using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /** Sort NodeUsage elements on their data elements.
     */
    class NodeUsageSorter<T> : IComparer<NodeUsage<T>>
    {
        private IComparer<T> sorter;

        public NodeUsageSorter(IComparer<T> sorter)
        {
            this.sorter = sorter;
        }

        public int Compare(NodeUsage<T> n1, NodeUsage<T> n2)
        {
            return sorter.Compare(n1.getData(), n2.getData());
        }
    }
}