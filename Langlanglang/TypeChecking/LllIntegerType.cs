using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Tokenization;

namespace Langlanglang.TypeChecking
{
    public class LllIntegerType : LllType
    {
        public int Size { get; }
        public bool Signed { get; }

        private LllIntegerType(string name, string cname, int size, bool signed)
            : base(name, cname, true, 0)
        {
            Size = size;
            Signed = signed;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LllIntegerType;
            if (other == null)
            {
                return base.Equals(obj);
            }

            return Signed == other.Signed
                   && Size == other.Size
                   && PointerDepth == other.PointerDepth;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool TryCast(LllType toType)
        {
            // IMPLICIT TYPE COERCION FOR INTEGER TYPES:
            // if the from-type (this), is unsigned it is only allowed to up-cast to the next tier
            // if the to-type is signed.
            // if the from-type (this), is signed, it is only allowed to cast to other signed types.

            var tt = toType as LllIntegerType;
            if (tt == null)
            {
                return false;
            }

            if (Equals(tt))
            {
                return true;
            }

            // disallow casting pointers to actual integer types
            if (tt.PointerDepth != 0 || PointerDepth != 0)
            {
                return false;
            }

            // disallow casting a signed type to an unsigned type.
            if (Signed && !tt.Signed)
            {
                return false;
            }

            // up-casting unsigned to a larger signed type.
            if (!Signed && tt.Signed)
            {
                return tt.Size > Size;
            }

            // Up or side-casting for same-signed integers
            return tt.Size >= Size;
        }

        public override LllType Clone(int withPtrDepth)
        {
            return new LllIntegerType(Name, CName, Size, Signed)
            {
                PointerDepth = withPtrDepth
            };
        }

        public override List<LllType> Select()
        {
            return new List<LllType> {Char, I8, U8, I16, U16, I32, U32, I64, U64};
        }

        public static readonly LllIntegerType Char = new LllIntegerType("char", "char", 1, true);

        public static readonly LllIntegerType I8 = new LllIntegerType("i8", "int8_t", 1, true);
        public static readonly LllIntegerType I16 = new LllIntegerType("i16", "int16_t", 2, true);
        public static readonly LllIntegerType I32 = new LllIntegerType("i32", "int32_t", 4, true);
        public static readonly LllIntegerType I64 = new LllIntegerType("i64", "int64_t", 8, true);

        public static readonly LllIntegerType U8 = new LllIntegerType("u8", "uint8_t", 1, false);
        public static readonly LllIntegerType U16 = new LllIntegerType("u16", "uint16_t", 2, false);
        public static readonly LllIntegerType U32 = new LllIntegerType("u32", "uint32_t", 4, false);
        public static readonly LllIntegerType U64 = new LllIntegerType("u64", "uint64_t", 8, false);
    }
}
