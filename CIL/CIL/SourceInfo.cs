using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL
{
    public class SourceInfo
    {
        public string FilePath { get; }
        public int Line { get; }
        public int Column { get; }

        public SourceInfo(string filePath, int line, int column)
        {
            FilePath = filePath.Replace("\\", "\\\\");
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return string.Format("{0} : L{1}:C{2}", FilePath, Line, Column);
        }
    }
}
