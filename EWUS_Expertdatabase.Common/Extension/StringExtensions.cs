using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Helper functions for String not already found in C#.
    /// Inspired by PHP String Functions that are missing in .Net.
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsJsonString(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if ((value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]")))
            {
                return true;
            }

            return false;

        }
        /// <summary>
        ///  Convert string to nullable long with default value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this string value, long defaultValue = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                long output = 0;

                if (Int64.TryParse(value, out output))
                    return output;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        // <summary>
        ///  Convert string to nullable long with default value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this object value, long defaultValue = 0)
        {
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///  Convert string to bool with default value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string value, bool defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            try
            {
                return Boolean.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
