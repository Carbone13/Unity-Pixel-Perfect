using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /** A minimum bounding rectangle
     */
    public interface MBR
    {
        /** Get the minimum x value */
        double getMinX();

        /** Get the minimum y value */
        double getMinY();

        /** Get the maximum x value */
        double getMaxX();

        /** Get the maximum y value */
        double getMaxY();

        /** Return a new MBR that is the union of this mbr and the other 
         * @param mbr the MBR to create a union with
         */
        MBR union(MBR mbr);

        /** Check if the other MBR intersects this one */
        bool intersects(MBR other);

        /** Check if this MBR intersects the rectangle given by the object 
         *  and the MBRConverter.
         */
        bool intersects<T>(T t, MBRConverter<T> converter);
    }
}