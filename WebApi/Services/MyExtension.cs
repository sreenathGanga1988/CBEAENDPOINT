using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static String TrimUpper(this String str)
        {
            return str.Trim().ToUpper();
        }
        public static String[] ToCommaSeperatedArray(this String str)
        {
            return str.Replace(";", ",").Trim().Split(",");
        }
    }
}






