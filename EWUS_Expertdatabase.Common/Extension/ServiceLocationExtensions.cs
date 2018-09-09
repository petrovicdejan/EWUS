using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.ServiceLocation
{
    public static class ServiceLocationExtension
    {
        public static object TryGet(this IServiceLocator locator, Type type, string key = null)
        {
            try
            {
                if (locator == null)
                    return null;

                if (string.IsNullOrWhiteSpace(key))
                    return locator.GetInstance(type);
                else
                    return locator.GetInstance(type, key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static List<string>
            _ContainsServiceLocatorFaild = new List<string>();

        public static bool Contains<T>(this IServiceLocator locator, string key, out T output)
        {
            output = default(T);

            try
            {
                if (locator == null)
                    return false;

                if (_ContainsServiceLocatorFaild.Contains(typeof(T).FullName))
                    return false;

                if (string.IsNullOrWhiteSpace(key))
                    output = locator.GetInstance<T>();
                else
                    output = locator.GetInstance<T>(key);

                return output != null;
            }
            catch
            {
                if (_ContainsServiceLocatorFaild.Contains(typeof(T).FullName) == false)
                    _ContainsServiceLocatorFaild.Add(typeof(T).FullName);

                return false;
            }
        }

        public static T TryGet<T>(this IServiceLocator locator, string key = null)
        {
            try
            {
                if (locator == null)
                    return default(T);
                
                if (string.IsNullOrWhiteSpace(key))
                    return locator.GetInstance<T>();
                else
                    return locator.GetInstance<T>(key);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static IEnumerable<object> TryGetAll(this IServiceLocator locator, Type type)
        {
            try
            {
                if (locator == null)
                    return Enumerable.Empty<object>();

                return locator.GetAllInstances(type);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<object>();
            }
        }


        public static IEnumerable<T> TryGetAll<T>(this IServiceLocator locator)
        {
            try
            {
                if (locator == null)
                    return Enumerable.Empty<T>();

                return locator.GetAllInstances<T>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<T>();
            }
        }
    }
}
