using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.khelekore.prtree
{
    public class NestedFinder<T> : IEnumerator<T>
    {

        private MBR mbr;
        private MBRConverter<T> converter;

        private List<T> ts = new List<T>();
        private List<Node<T>> toVisit = new List<Node<T>>();
        private int visitedNodes = 0;
        private int dataNodesVisited = 0;

        private T current;
        private int index;


        public NestedFinder(Node<T> _root, MBR _mbr, MBRConverter<T> _converter)
        {
            converter = _converter;
            mbr = _mbr;
            toVisit.Add(_root);
            index = -1;
        }

        public T Current
        {
            get { return current; }
        }

        object IEnumerator.Current
        {
            get { return (object)Current; }
        }

        public void Dispose()
        {
            current = default(T);
            //index = coll.items.Length;
        }

        public bool MoveNext()
        {
            bool fail = true;
            while (ts.Count == 0 && toVisit.Count != 0)
            {
                Node<T> n = toVisit[toVisit.Count - 1];
                toVisit.RemoveAt(toVisit.Count - 1);
                visitedNodes++;
                n.expand(mbr, converter, ts, toVisit);
            }
            if (ts.Count == 0)
            {
                current = default(T);
                fail = false;
            }
            else
            {
                current = ts[ts.Count - 1];
                ts.RemoveAt(ts.Count - 1);
                dataNodesVisited++;
            }
            return fail;
        }

        public void Reset()
        {
            current = default(T);
            index = 0;
        }
    }
}
