using System;
using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /** A Priority R-Tree, a spatial index.
     *  This tree only supports bulk loading.
     *  I (C13) added a single insert method, but idk if it work
     *  <pre>{@code
     *  PRTree<Rectangle2D> tree = 
     *      new PRTree<Rectangle2D> (new Rectangle2DConverter (), 10);
     *  Rectangle2D rx = new Rectangle2D.Double (0, 0, 1, 1);
     *  tree.load (Collections.singletonList (rx));
     *  for (Rectangle2D r : tree.find (0, 0, 1, 1)) {
     *      System.out.println ("found a rectangle: " + r);
     *  }
     *  }</pre>
     * 
     * @param <T> the data type stored in the PRTree
     */
    public class PRTree<T> : IEnumerable<T>
    {

        private MBRConverter<T> converter;
        private int branchFactor;

        private Node<T> root;
        private int numLeafs;
        private int height;

        /** Create a new PRTree using the specified branch factor.
         * @param branchFactor the number of child nodes for each internal node.
         */
        public PRTree(MBRConverter<T> converter, int branchFactor)
        {
            this.converter = converter;
            this.branchFactor = branchFactor;
        }

        public Node<T> GetRoot() { return root; }

        private int estimateSize(int dataSize)
        {
            return (int)(1.0 / (branchFactor - 1) * dataSize);
        }

        /** Bulk load data into this tree.
         *
         *  Create the leaf nodes that each hold (up to) branchFactor data entries.
         *  Then use the leaf nodes as data until we can fit all nodes into 
         *  the root node.
         *
         * @param data the collection of data to store in the tree.
         * @throws IllegalStateException if the tree is already loaded
         */
        public void BulkInsert(List<T> data)
        {
            if (root != null) throw new Exception("Tree is already loaded");
            numLeafs = data.Count;
            XComparator<T> xSorter = new XComparator<T>(converter);
            YComparator<T> ySorter = new YComparator<T>(converter);
            List<LeafNode<T>> leafNodes = new List<LeafNode<T>>(estimateSize(numLeafs));
            LeafBuilder lb = new LeafBuilder(branchFactor);
            lb.buildLeafs(data, leafNodes, xSorter, ySorter, new LeafNodeFactory());

            height = 1;
            if (leafNodes.Count < branchFactor)
            {
                SetRoot(leafNodes);
            }
            else
            {
                XNodeComparator<T> xs = new XNodeComparator<T>(converter);
                YNodeComparator<T> ys = new YNodeComparator<T>(converter);
                List<Node<T>> nodes = leafNodes.ConvertAll(TypeConverter.UpCast<LeafNode<T>, Node<T>>());
                //List<LeafNode<T>> nodes = leafNodes;
                do
                {
                    height++;
                    int es = estimateSize(nodes.Count);
                    List<InternalNode<T>> internalNodes = new List<InternalNode<T>>(es);
                    lb.buildLeafs(nodes, internalNodes, xs, ys, new InternalNodeFactory());
                    nodes = internalNodes.ConvertAll(TypeConverter.UpCast<InternalNode<T>, Node<T>>());
                } while (nodes.Count > branchFactor);
                SetRoot(nodes);
            }
        }

        /** Get a minimum bounding rectangle of the data stored in this tree.
         */
        public MBR GetMBR()
        {
            return root.getMBR(converter);
        }

        /** Get the number of data leafs in this tree. 
         */
        public int GetNumberOfLeaves()
        {
            return numLeafs;
        }

        /** Get the height of this tree. 
         */
        public int GetHeight()
        {
            return height;
        }

        private void SetRoot<N>(List<N> nodes) where N : Node<T>
        {
            if (nodes.Count == 0)
                root = new InternalNode<T>(new object[0]);
            else if (nodes.Count == 1)
            {
                root = nodes[0];
            }
            else
            {
                height++;
                root = new InternalNode<T>(nodes.ToArray());
            }
        }

        private class LeafNodeFactory : LeafBuilder.NodeFactory<LeafNode<T>>
        {
            public LeafNode<T> create(object[] data)
            {
                return new LeafNode<T>(data);
            }
        }

        private class InternalNodeFactory : LeafBuilder.NodeFactory<InternalNode<T>>
        {
            public InternalNode<T> create(object[] data)
            {
                return new InternalNode<T>(data);
            }
        }

        private void validateRect(double xmin, double ymin, double xmax, double ymax)
        {
            if (xmax < xmin)
                throw new Exception("xmax: " + xmax + " < xmin: " + xmin);
            if (ymax < ymin)
                throw new Exception("ymax: " + ymax + " < ymin: " + ymin);
        }

        /** Finds all objects that intersect the given rectangle and stores
         *  the found node in the given list.
         * @param resultNodes the list that will be filled with the result
         */
        public void Find(double xmin, double ymin, double xmax, double ymax, List<T> resultNodes)
        {
            MBR mbr = new SimpleMBR(xmin, ymin, xmax, ymax);
            Find(mbr, resultNodes);
        }

        /** Finds all objects that intersect the given rectangle and stores
         *  the found node in the given list.
         * @param resultNodes the list that will be filled with the result
         */
        public void Find(MBR query, List<T> resultNodes)
        {
            validateRect(query.getMinX(), query.getMinY(), query.getMaxX(), query.getMaxY());
            root.find(query, converter, resultNodes);
        }

        /** Find all objects that intersect the given rectangle.
         * @throws IllegalArgumentException if xmin &gt; xmax or ymin &gt; ymax
         */

        //public NestedFinder<T> find(MBR query)
        public IEnumerable<T> Find(MBR query)
        { //Iterable
            validateRect(query.getMinX(), query.getMinY(), query.getMaxX(), query.getMaxY());
            _tempMBR = query;
            return this;

        }

        /** Find all objects that intersect the given rectangle.
         * @throws IllegalArgumentException if xmin &gt; xmax or ymin &gt; ymax
         */
        //public NestedFinder<T> find(double xmin, double ymin, double xmax, double ymax)
        public IEnumerable<T> Find(double xmin, double ymin, double xmax, double ymax)
        { //Iterable
            MBR mbr = new SimpleMBR(xmin, ymin, xmax, ymax);
            return Find(mbr);
        }

        private MBR _tempMBR;

        public IEnumerator<T> GetEnumerator()
        {
            return new NestedFinder<T>(root, _tempMBR, converter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}