using System;
using System.Linq.Expressions;
using System.Text;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstNumber : AstExpression
    {
        public decimal Number { get; set; }

        public AstNumber(SourceInfo si, decimal number)
            : base(si)
        {
            Number = number;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            if (Number%1 == 0 && Math.Abs(Number) <= long.MaxValue)
            {
                return new CILInteger(SourceInfo, (long)Number);
            }
            return new CILFloatingPoint(SourceInfo, (double)Number);
        }

        private bool IsBetweenInclusive(decimal min, decimal max)
        {
            return Number >= min && Number <= max;
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            if (Number%1 == 0)
            {
                //if (IsBetweenInclusive(sbyte.MinValue, sbyte.MaxValue))
                //{
                //    return LllCompiler.SymTable.LookupType("i8");
                //}
                //if (IsBetweenInclusive(short.MinValue, short.MaxValue))
                //{
                //    return LllCompiler.SymTable.LookupType("short");
                //}
                if (IsBetweenInclusive(int.MinValue, int.MaxValue))
                {
                    return LllCompiler.SymTable.LookupType("int");
                }
                if (IsBetweenInclusive(long.MinValue, long.MaxValue))
                {
                    return LllCompiler.SymTable.LookupType("long");
                }
            }
            if (Number > Convert.ToDecimal(double.MaxValue)
                || Number < Convert.ToDecimal(double.MinValue))
            {
                throw new NotImplementedException("TODO: Exception for number overflow");
            }
            return LllCompiler.SymTable.LookupType("double");
        }
    }
}
