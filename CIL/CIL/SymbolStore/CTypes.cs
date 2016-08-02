using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CIL.Exceptions;

namespace CIL.SymbolStore
{
    public static class CTypes
    {
        // Matches integer types from SymbolType.CreateBasic()
        private static readonly Regex DefinedSizeIntegers = new Regex("^(u?int(8|16|32|64)_t)$");
        private static readonly Regex UndefinedSizeIntegers = new Regex(@"^(char|short|int|size_t)$");
        private static readonly Regex PointerRegex = new Regex(@"^(struct\s+)?\w+(\s*\*)+$");

        public static bool IsIntegerType(string type)
        {
            return UndefinedSizeIntegers.IsMatch(type) 
                || IsDefinedSizeIntegerType(type) 
                || IsPointerType(type);
        }

        public static bool IsDefinedSizeIntegerType(string type)
        {
            return DefinedSizeIntegers.IsMatch(type);
        }

        public static bool IsFloatingType(string type)
        {
            return type == "float" || type == "double";
        }

        public static bool IsPointerType(string type)
        {
            return PointerRegex.IsMatch(type);
        }

        public static string LargerIntegerType(string ty1, string ty2)
        {
            if (!IsIntegerType(ty1) || !IsIntegerType(ty2))
            {
                throw new NotImplementedException("TODO: Exception for trying to up-cast a non-integer to an integer");
            }
            if (ty1 == ty2)
            {
                return ty1;
            }
            if (IsPointerType(ty1) && IsPointerType(ty2))
            {
                return "size_t";
            }
            // the below is not portable
            // note: the order of these matters, the comparisons should be ordered
            //       from the largest types to the smallest types.
            if (IsDefinedSizeIntegerType(ty1) && IsDefinedSizeIntegerType(ty2))
            {
                var sign = (ty1[0] == 'i' || ty2[0] == 'i') ? "" : "u";
                if (ty1.Contains("64") || ty2.Contains("64")) { return sign + "int64_t"; }
                if (ty1.Contains("32") || ty2.Contains("32")) { return sign + "int32_t"; }
                if (ty1.Contains("16") || ty2.Contains("16")) { return sign + "int16_t"; }
                return sign + "int8_t";
            }
            if (ty1 == "int" || ty2 == "int") { return "int"; }
            if (ty1 == "short" || ty2 == "short") { return "short"; }
            if (ty1 == "char" || ty2 == "char") { return "char"; }
            return "size_t";
        }

        public static string LargerFloatingType(string ty1, string ty2)
        {
            if (!IsFloatingType(ty1) || !IsFloatingType(ty2))
            {
                throw new NotImplementedException("TODO: Exception for floating mismatch");
            }
            if (ty1 == ty2)
            {
                return ty1;
            }
            if (ty1 == "double" || ty2 == "double")
            {
                return "double";
            }
            return "float";
        }

        public static string BestNumberType(string ty1, string ty2)
        {
            if (IsIntegerType(ty1) && IsIntegerType(ty2))
            {
                return LargerIntegerType(ty1, ty2);
            }
            if (IsFloatingType(ty1) && IsFloatingType(ty2))
            {
                return LargerFloatingType(ty1, ty2);
            }
            if (IsFloatingType(ty1) && IsIntegerType(ty2))
            {
                return ty1;
            }
            if (IsFloatingType(ty2) && IsIntegerType(ty1))
            {
                return ty2;
            }
            throw new NotImplementedException("TODO: Exception for trying to convert a non-number to a number");
        }
    }
}
