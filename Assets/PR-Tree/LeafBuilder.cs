using System;
using System.Collections;
using System.Collections.Generic;

/** A builder of internal nodes used during bulk loading of a PR-Tree.
 *  A PR-Tree is build by building a pseudo R-Tree and grabbing the
 *  leaf nodes (and then repeating until you have just one root node).
 *  This class creates the leaf nodes without building the full pseudo tree.
 */
namespace org.khelekore.prtree
{
    class LeafBuilder
    {

        private int branchFactor;
        private int listHolderId;

        public LeafBuilder(int branchFactor)
        {
            this.branchFactor = branchFactor;
        }

        /** A factory that creates the nodes (either leaf or internal).
         */
        public interface NodeFactory<N>
        {
            /** Create a new node 
             * @param data the data entries for the node, fully filled.
             */
            N create(object[] data);
        }

        public void buildLeafs<X, T, N>(List<X> ls, List<N> leafNodes, IComparer<T> xSorter, IComparer<T> ySorter, NodeFactory<N> nf)
            where X : T
        {
            listHolderId = 1;
            List<NodeUsage<T>> lsx = new List<NodeUsage<T>>(ls.Count);
            List<NodeUsage<T>> lsy = new List<NodeUsage<T>>(ls.Count);

            foreach (T tr in ls)
            {
                NodeUsage<T> nu = new NodeUsage<T>(tr);
                lsx.Add(nu);
                lsy.Add(nu);
            }

            lsx.Sort(new NodeUsageSorter<T>(xSorter));
            lsy.Sort(new NodeUsageSorter<T>(ySorter));
            List<NodeGetter<T>> toExpand = new List<NodeGetter<T>>();
            ListData<T> listData = new ListData<T>(lsx, lsy);
            int top = lsx.Count - 1;
            toExpand.Add(new NodeGetter<T>(this, listData, listHolderId++, lsx.Count, 0, 0, top, top)); ;
            internalBuildLeafs(toExpand, leafNodes, nf);
        }

        private class ListData<T>
        {
            // Same NodeUsage objects in both lists, just ordered differently.
            public List<NodeUsage<T>> sx;
            public List<NodeUsage<T>> sy;

            public ListData(List<NodeUsage<T>> sx, List<NodeUsage<T>> sy)
            {
                this.sx = sx;
                this.sy = sy;
            }

            public string ToString()
            {
                return GetType().ToString() + "{sx: " + sx + ", sy: " + sy + "}";
            }
        }

        private class NodeGetter<T>
        {
            private ListData<T> data;
            private int taken = 0;
            private int size;
            private int id;

            // indexes for list scanning
            private int xlow;  // goes up as we pick low nodes
            private int ylow;
            private int xhigh; // goes down as we pick high nodes
            private int yhigh;

            // we're not in Java anymore, internal classes need the home pattern
            private LeafBuilder home;

            /** 
             * @param data the lists to grab node data from
             * @param id the id of the nodes we may pick
             * @param size the number of nodes we may pick
             * @param xlow the lower start index for the x list 
             * @param ylow the lower start index for the x list
             * @param xhigh the upper start index for the x list
             * @param yhigh the upper start index for the x list
             */
            public NodeGetter(LeafBuilder home, ListData<T> data, int id, int size, int xlow, int ylow, int xhigh, int yhigh)
            {
                this.home = home;
                this.data = data;
                this.id = id;
                this.size = size;
                this.xlow = xlow;
                this.ylow = ylow;
                this.xhigh = xhigh;
                this.yhigh = yhigh;
            }

            public string ToString()
            {
                return GetType().ToString() + "{taken: " + taken +
                ", size: " + size + ", id: " + id + ", xlow: " + xlow +
                ", ylow: " + ylow + ", xhigh: " + xhigh + ", yhigh: " +
                yhigh + ", data: " + data + "}";
            }

            public bool hasMoreData()
            {
                return elementsLeft() > 0;
            }

            public int elementsLeft()
            {
                return size - taken;
            }

            private bool isUsedNode(List<NodeUsage<T>> ls, int pos)
            {
                NodeUsage<T> nu = ls[pos];
                return nu == null || nu.isUsed() || nu.getUser() != id;
            }

            private NodeUsage<T> getFirstUnusedXNode()
            {
                while (xlow < xhigh && isUsedNode(data.sx, xlow))
                    xlow++;
                return data.sx[xlow++];
            }

            public T getFirstUnusedX()
            {
                taken++;
                NodeUsage<T> nu = getFirstUnusedXNode();
                nu.use();
                data.sx[xlow - 1] = null;
                return nu.getData();
            }

            public T getFirstUnusedY()
            {
                taken++;
                while (ylow < yhigh && isUsedNode(data.sy, ylow))
                    ylow++;
                NodeUsage<T> nu = data.sy[ylow];
                data.sy[ylow++] = null;
                nu.use();
                return nu.getData();
            }

            private NodeUsage<T> getLastUnusedXNode()
            {
                while (xhigh > xlow && isUsedNode(data.sx, xhigh))
                    xhigh--;
                return data.sx[xhigh--];
            }

            public T getLastUnusedX()
            {
                taken++;
                NodeUsage<T> nu = getLastUnusedXNode();
                nu.use();
                data.sx[xlow - 1] = null;
                return nu.getData();
            }

            public T getLastUnusedY()
            {
                taken++;
                while (yhigh > ylow && isUsedNode(data.sy, yhigh))
                    yhigh--;
                NodeUsage<T> nu = data.sy[yhigh];
                data.sy[yhigh--] = null;
                nu.use();
                return nu.getData();
            }

            /** Split the remaining data into two parts,
             *  one part with the low x values and one with the high x values.
             */
            public List<NodeGetter<T>> split()
            {
                int e = elementsLeft();
                int lowSize = (e + 1) / 2;
                int highSize = e - lowSize;
                int lowId = home.listHolderId++;
                int highId = home.listHolderId++;

                // save positions
                int xl = xlow;
                int xh = xhigh;

                // pick a low element to the low list, mark as low,
                // pick a high element to the high list, mark as high
                while (hasMoreData())
                {
                    taken++;
                    NodeUsage<T> nu = getFirstUnusedXNode();
                    nu.setUser(lowId);
                    if (hasMoreData())
                    {
                        taken++;
                        nu = getLastUnusedXNode();
                        nu.setUser(highId);
                    }
                }

                NodeGetter<T> lhLow = new NodeGetter<T>(home, data, lowId, lowSize, xl, ylow, xhigh, yhigh);
                NodeGetter<T> lhHigh = new NodeGetter<T>(home, data, highId, highSize, xlow, ylow, xh, yhigh);
                List<NodeGetter<T>> ret = new List<NodeGetter<T>>(2);
                ret.Add(lhLow);
                ret.Add(lhHigh);
                return ret;
            }
        }

        /** Construct the four edge nodes then split the rests of the nodes 
         *  in the middle and loop with the two middle sets.
         */
        private void internalBuildLeafs<T, N>(List<NodeGetter<T>> toExpand,
                            List<N> leafNodes,
                            NodeFactory<N> nf)
        {
            while (toExpand.Count != 0)
            {
                NodeGetter<T> lh = toExpand[0];
                toExpand.RemoveAt(0);
                if (lh.hasMoreData())
                    leafNodes.Add(getLowXNode(lh, nf));

                if (lh.hasMoreData())
                    leafNodes.Add(getLowYNode(lh, nf));

                if (lh.hasMoreData())
                    leafNodes.Add(getHighXNode(lh, nf));

                if (lh.hasMoreData())
                    leafNodes.Add(getHighYNode(lh, nf));

                if (lh.hasMoreData())
                {
                    List<NodeGetter<T>> splitted = lh.split();
                    toExpand.AddRange(splitted);
                }
            }
        }

        /** Get the size of the node we are about to create, 
         *  limit by elements left and by branchfactor
         */
        private int getNum<T>(NodeGetter<T> lh)
        {
            return Math.Min(lh.elementsLeft(), branchFactor);
        }

        private N getLowXNode<T, N>(NodeGetter<T> lh, NodeFactory<N> nf)
        {
            object[] data = new object[getNum(lh)];
            for (int i = 0; i < data.Length; i++)
                data[i] = lh.getFirstUnusedX();
            return nf.create(data);
        }

        private N getLowYNode<T, N>(NodeGetter<T> lh, NodeFactory<N> nf)
        {
            object[] data = new object[getNum(lh)];
            for (int i = 0; i < data.Length; i++)
                data[i] = lh.getFirstUnusedY();
            return nf.create(data);
        }

        private N getHighXNode<T, N>(NodeGetter<T> lh, NodeFactory<N> nf)
        {
            object[] data = new object[getNum(lh)];
            for (int i = 0; i < data.Length; i++)
                data[i] = lh.getLastUnusedX();
            return nf.create(data);
        }

        private N getHighYNode<T, N>(NodeGetter<T> lh, NodeFactory<N> nf)
        {
            object[] data = new object[getNum(lh)];
            for (int i = 0; i < data.Length; i++)
                data[i] = lh.getLastUnusedY();
            return nf.create(data);
        }
    }
}