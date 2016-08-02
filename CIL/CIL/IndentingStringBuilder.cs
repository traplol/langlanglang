using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL
{
    public class IndentingStringBuilder
    {
        private readonly StringBuilder _sb;
        private int _indentionLevel;
        public int Capacity => _sb.Capacity;
        public int MaxCapacity => _sb.MaxCapacity;
        public int Length => _sb.Length;

        public IndentingStringBuilder()
        {
            _sb = new StringBuilder();
        }

        public IndentingStringBuilder(int capacity)
        {
            _sb = new StringBuilder(capacity);
        }

        public IndentingStringBuilder(int capacity, int maxCapacity)
        {
            _sb = new StringBuilder(capacity, maxCapacity);
        }

        public IndentingStringBuilder(string value)
        {
            _sb = new StringBuilder(value);
        }

        public IndentingStringBuilder(string value, int capacity)
        {
            _sb = new StringBuilder(value, capacity);
        }

        public void Indent()
        {
            _indentionLevel++;
        }

        public void Dedent()
        {
            _indentionLevel--;
            if (_indentionLevel < 0)
            {
                throw new NotImplementedException("TODO: Exception for negative indention level");
            }

        }

        private void DoIndention()
        {
            _sb.Append(new string(' ', 4*_indentionLevel));
        }

        public void Append(object obj)
        {
            DoIndention();
            _sb.Append(obj);
        }

        public void Append(string str)
        {
            DoIndention();
            _sb.Append(str);
        }

        public void AppendLine(object obj)
        {
            DoIndention();
            _sb.AppendLine(obj.ToString());
        }

        public void AppendLine(string str)
        {
            DoIndention();
            _sb.AppendLine(str);
        }

        public void AppendNoIndent(string str)
        {
            _sb.Append(str);
        }

        public void AppendLineNoIndent(string str)
        {
            _sb.AppendLine(str);
        }

        public void LineDecl(SourceInfo si)
        {
            if (false && si.Line >= 0)
            {
                _sb.AppendLine(string.Format("#line {0} \"{1}\"", si.Line, si.FilePath));
            }
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
