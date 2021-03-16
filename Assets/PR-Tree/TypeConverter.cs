using System;
using System.Collections.Generic;

namespace org.khelekore.prtree
{
    public class TypeConverter
    {
        /// <summary>        
        /// Returns a delegate that can be used to cast a subtype back to its base type.         
        /// </summary>        
        /// <typeparam name="T">The derived type</typeparam>        
        /// <typeparam name="U">The base type</typeparam>        
        /// <returns>Delegate that can be used to cast a subtype back to its base type. </returns>        
        public static Converter<T, U> UpCast<T, U>() where T : U
        {
            return delegate(T item) { return (U)item; };
        }

        public static Converter<T, U> DownCast<T, U>() where U : T
        {
            return delegate(T item) { return (U)item; };
        }
    }
}
