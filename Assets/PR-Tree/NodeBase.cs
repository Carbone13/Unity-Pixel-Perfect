using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /**
     * @param N the type of the child entries
     * @param T the type of the data entries
     */
    abstract class NodeBase<N, T> : Node<T>
    {
        private MBR mbr;
        private Array data;

        public NodeBase(Array data)
        {
            this.data = data;
        }

        public int size()
        {
            return data.Length;
        }

        //@SuppressWarnings("unchecked")
        public N get(int i)
        {
            return (N)((Array)data).GetValue(i);
        }

        public MBR getMBR(MBRConverter<T> converter)
        {
            if (mbr == null) mbr = computeMBR(converter);
            return mbr;
        }

        public abstract MBR computeMBR(MBRConverter<T> converter);

        public MBR getUnion(MBR m1, MBR m2)
        {
            if (m1 == null) return m2;
            return m1.union(m2);
        }

        public virtual void expand(MBR mbr, MBRConverter<T> converter, List<T> found, List<Node<T>> nodesToExpand)
        {
            throw new System.NotImplementedException();
        }

        public virtual void find(MBR mbr, MBRConverter<T> converter, List<T> result)
        {
            throw new System.NotImplementedException();
        }

        public virtual void jExpand(List<T> found, List<Node<T>> nodesToExpand)
        {
            throw new System.NotImplementedException();
        }
        public virtual void jFind(List<T> result)
        {
            throw new System.NotImplementedException();
        }
        public virtual void toJSON(StringBuilder sb, MBRConverter<T> converter)
        {
            throw new System.NotImplementedException();
        }
        public virtual void toJSON(StringBuilder sb, MBRConverter<T> converter, string format)
        {
            throw new System.NotImplementedException();
        }

    }
}