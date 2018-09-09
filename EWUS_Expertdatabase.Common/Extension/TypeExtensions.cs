using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {
        public static bool InheritsFrom<T>(this Type t)
        {
            return t.InheritsFrom(typeof(T));
        }

        /// <summary>
        /// Extension method to check the entire inheritance hierarchy of a
        /// type to see whether the given base type is inherited.
        /// </summary>
        /// <param name="t">The Type object this method was called on</param>
        /// <param name="baseType">The base type to look for in the 
        /// inheritance hierarchy</param>
        /// <returns>True if baseType is found somewhere in the inheritance 
        /// hierarchy, false if not</returns>
        public static bool InheritsFrom(this Type t, Type baseType)
        {
            if (t == baseType)
                return true;

            if (baseType.IsInterface)
            {
                var items = t.GetInterfaces();

                if (items != null && items.Contains(baseType))
                    return true;
            }

            Type cur = t.BaseType;

            while (cur != null)
            {
                if (baseType.IsGenericType && cur.IsGenericType)
                {
                    if (cur.GetGenericTypeDefinition() == baseType)
                        return true;
                }

                if (baseType.IsInterface)
                {
                    var items = cur.GetInterfaces();

                    if (items != null && items.Contains(baseType))
                        return true;
                }

                if (cur.Equals(baseType))
                {
                    return true;
                }

                cur = cur.BaseType;
            }

            return false;
        }
    }
}
