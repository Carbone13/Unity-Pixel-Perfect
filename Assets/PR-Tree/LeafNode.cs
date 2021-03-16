using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace org.khelekore.prtree
{
    class LeafNode<T> : NodeBase<T, T>
    {

        public LeafNode(object[] data) : base(data) { }

        public MBR getMBR(T t, MBRConverter<T> converter)
        {
            return new SimpleMBR(converter.getMinX(t), converter.getMinY(t), converter.getMaxX(t), converter.getMaxY(t));
        }

        public override MBR computeMBR(MBRConverter<T> converter)
        {
            MBR ret = null;
            for (int i = 0, s = size(); i < s; i++)
                ret = getUnion(ret, getMBR(get(i), converter));
            return ret;
        }


        public override void expand(MBR mbr, MBRConverter<T> converter, List<T> found, List<Node<T>> nodesToExpand)
        {
            find(mbr, converter, found);
        }

        public override void find(MBR mbr, MBRConverter<T> converter, List<T> result)
        {
            for (int i = 0, s = size(); i < s; i++)
            {
                T t = get(i);
                if (mbr.intersects(t, converter)) result.Add(t);
            }
        }

        public override void toJSON(StringBuilder sb, MBRConverter<T> converter)
        {
            toJSON(sb, converter, "{0}");
        }


        public override void toJSON(StringBuilder sb, MBRConverter<T> converter, string format)
        {
            for (int i = 0, s = size() - 1; i <= s; i++)
            {
                T leaf = get(i);
                double xmin = converter.getMinX(leaf);
                double ymin = converter.getMinY(leaf);
                double xmax = converter.getMaxX(leaf);
                double ymax = converter.getMaxY(leaf);
                if (xmin == xmax && ymin == ymax)
                    sb.Append("{b:[" + string.Format(format, xmin) + "," + string.Format(format, ymin) + "],");
                else
                    sb.Append("{b:[" + string.Format(format, xmin) + "," + string.Format(format, ymin) + "," + string.Format(format, xmax) + "," + string.Format(format, ymax) + "],");
                sb.Append("i:" + get(i).ToString());
                sb.Append("}");
                if (i != s) sb.Append(",");
            }
        }
    }
}