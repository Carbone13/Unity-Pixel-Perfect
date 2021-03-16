using System;
using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /** Information needed to be able to figure out how an
     *  element of the tree is currently used.
     */
    class NodeUsage<T>
    {
        /** The actual data of the node. */
        private T data;
        /** The leaf node builder user id (split id). */
        private int usage = 1;

        public NodeUsage(T data)
        {
            this.data = data;
        }

        public T getData()
        {
            return data;
        }

        public void use()
        {
            if (usage >= 0)
                usage = -usage;
            else
                throw new Exception("using already used node");
        }

        public bool isUsed()
        {
            return usage < 0;
        }

        public void setUser(int id)
        {
            if (id < 0) throw new Exception("id must be positive");
            usage = id;
        }

        public int getUser()
        {
            return Math.Abs(usage);
        }

        public string ToString()
        {
            return GetType().ToString() + "{data: " + data + ", used: " + isUsed() + ", user: " + getUser() + "}";
        }
    }
}