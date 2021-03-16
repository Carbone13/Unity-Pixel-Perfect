using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace org.khelekore.prtree
{
    class InternalNode<T> : NodeBase<Node<T>, T>
    {
        public InternalNode(Array data) : base(data) { }

        public override MBR computeMBR(MBRConverter<T> converter)
        {
            MBR ret = null;
            for (int i = 0, s = size(); i < s; i++)
                ret = getUnion(ret, get(i).getMBR(converter));
            return ret;
        }

        public override void expand(MBR mbr, MBRConverter<T> converter, List<T> found, List<Node<T>> nodesToExpand)
        {
            for (int i = 0, s = size(); i < s; i++)
            {
                Node<T> n = get(i);
                if (mbr.intersects(n.getMBR(converter)))
                    nodesToExpand.Add(n);
            }
        }

        public override void find(MBR mbr, MBRConverter<T> converter, List<T> result)
        {
            for (int i = 0, s = size(); i < s; i++)
            {
                Node<T> n = get(i);
                if (mbr.intersects(n.getMBR(converter)))
                    n.find(mbr, converter, result);
            }
        }

        public override void toJSON(StringBuilder sb, MBRConverter<T> converter)
        {
            toJSON(sb, converter, "{0}");
        }


        public override void toJSON(StringBuilder sb, MBRConverter<T> converter, string format)
        {
            MBR bounds = getMBR(converter);
            double xmin = bounds.getMinX();
            double ymin = bounds.getMinY();
            double xmax = bounds.getMaxX();
            double ymax = bounds.getMaxY();
            sb.Append("{b:[" + string.Format(format, xmin) + "," + string.Format(format, ymin) + "," + string.Format(format, xmax) + "," + string.Format(format, ymax) + "]");
            if (size() > 0)
            {
                sb.Append(",c:[");
                for (int i = 0, s = size() - 1; i <= s; i++)
                {
                    get(i).toJSON(sb, converter, format);
                    if (i != s) sb.Append(",");
                }
                sb.Append("]");
            }
            sb.Append("}");
        }

    }
}
