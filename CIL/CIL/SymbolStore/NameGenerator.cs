using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.SymbolStore
{
    public static class NameGenerator
    {
        private static int _id;

        public static string NewTemp()
        {
            return "__tmp" + _id++;
        }

        public static string NewLabel()
        {
            return "L" + _id++;
        }

        public static string UniqName(string prefix, string name)
        {
            return string.Format("{0}_{1}_{2}", prefix, name,  _id++);
        }
    }
}
