using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace org.khelekore.prtree
{
    /** A node in a Priority R-Tree
     */
    public interface Node<T>
    {
        /** Get the size of the node, that is how many data elements it holds */
        int size();

        /** Get the MBR of this node */
        MBR getMBR(MBRConverter<T> converter);

        /** Visit this node and add the leafs to the found list and add 
         *  any child nodes to the list of nodes to expand.
         */
        void expand(MBR mbr, MBRConverter<T> converter,
             List<T> found, List<Node<T>> nodesToExpand);

        /** Find nodes that intersect with the given MBR.
         */
        void find(MBR mbr, MBRConverter<T> converter, List<T> result);

        /** Get a JSON representation of this node. */
        void toJSON(StringBuilder sb, MBRConverter<T> converter);

        /** Get a JSON representation of this node. */
        void toJSON(StringBuilder sb, MBRConverter<T> converter, string format);


    }
}
