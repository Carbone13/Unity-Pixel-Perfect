using System;
using System.Collections;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    /** An implementation of MBR that keeps 4 double values for the actual min and
     *  max values needed.
     */
    public class SimpleMBR : MBR
    {
        private double xmin;
        private double ymin;
        private double xmax;
        private double ymax;

        public SimpleMBR(double xmin, double ymin, double xmax, double ymax)
        {
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
        }

        /** Get a string representation of this mbr. 
         */
        public override string ToString()
        {
            return GetType().ToString() + "{xmin: " + xmin + ", ymin: " + ymin + ", xmax: " + xmax + ", ymax: " + ymax + "}";
        }

        public double getMinX()
        {
            return xmin;
        }

        public double getMinY()
        {
            return ymin;
        }

        public double getMaxX()
        {
            return xmax;
        }

        public double getMaxY()
        {
            return ymax;
        }

        public MBR union(MBR other)
        {
            double uxmin = Math.Min(xmin, other.getMinX());
            double uymin = Math.Min(ymin, other.getMinY());
            double uxmax = Math.Max(xmax, other.getMaxX());
            double uymax = Math.Max(ymax, other.getMaxY());
            return new SimpleMBR(uxmin, uymin, uxmax, uymax);
        }

        public bool intersects(MBR other)
        {
            return !(other.getMaxX() < xmin || other.getMinX() > xmax || other.getMaxY() < ymin || other.getMinY() > ymax);
        }

        public bool intersects<T>(T t, MBRConverter<T> converter)
        {
            return !(converter.getMaxX(t) < xmin || converter.getMinX(t) > xmax || converter.getMaxY(t) < ymin || converter.getMinY(t) > ymax);
        }
    }
}