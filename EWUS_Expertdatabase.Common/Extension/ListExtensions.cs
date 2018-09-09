using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public static class IListExtensions
    {
        public static int Count(this IEnumerable source)
        {
            int res = 0;

            if (source == null)
                return res;

            foreach (var item in source)
                res++;

            return res;
        }
    }
}
