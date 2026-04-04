using System.Text;

namespace Kaitai
{
    public abstract class KaitaiStruct
    {
        protected KaitaiStream m_io;

        public KaitaiStream M_Io
        {
            get
            {
                return m_io;
            }
        }

        public KaitaiStruct(KaitaiStream io)
        {
            m_io = io;
        }
    }

    /// <summary>
    /// A custom decoder interface. Implementing classes can be called from
    /// inside a .ksy file using `process: XXX` syntax.
    /// </summary>
    public interface CustomDecoder
    {
        /// <summary>
        /// Decodes a given byte array, according to some custom algorithm
        /// (specific to implementing class) and parameters given in the
        /// constructor, returning another byte array.
        /// </summary>
        /// <param name="src">Source byte array.</param>
        byte[] Decode(byte[] src);
    }

    /// <summary>
    /// Error that occurs when default endianness should be decided with a
    /// switch, but nothing matches (although using endianness expression
    /// implies that there should be some positive result).
    /// </summary>
    public class UndecidedEndiannessError : Exception
    {
        public UndecidedEndiannessError()
            : base("Unable to decide on endianness")
        {
        }
        public UndecidedEndiannessError(string msg)
            : base(msg)
        {
        }
        public UndecidedEndiannessError(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }

    /// <summary>
    /// Common ancestor for all error originating from Kaitai Struct usage.
    /// Stores KSY source path, pointing to an element supposedly guilty of
    /// an error.
    /// </summary>
    public class KaitaiStructError : Exception
    {
        public KaitaiStructError(string msg, string srcPath)
            : base(srcPath + ": " + msg)
        {
            this.srcPath = srcPath;
        }

        protected string srcPath;
    }

    /// <summary>
    /// Common ancestor for all validation failures. Stores pointer to
    /// KaitaiStream IO object which was involved in an error.
    /// </summary>
    public class ValidationFailedError : KaitaiStructError
    {
        public ValidationFailedError(string msg, KaitaiStream io, string srcPath)
            : base("at pos " + io.Pos + ": validation failed: " + msg, srcPath)
        {
            this.io = io;
        }

        protected KaitaiStream io;

        protected static string ByteArrayToHex(byte[] arr)
        {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < arr.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(string.Format("{0:X2}", arr[i]));
            }
            sb.Append(']');
            return sb.ToString();
        }
    }

    /// <summary>
    /// Signals validation failure: we required "actual" value to be equal to
    /// "expected", but it turned out that it's not.
    /// </summary>
    public class ValidationNotEqualError : ValidationFailedError
    {
        public ValidationNotEqualError(byte[] expected, byte[] actual, KaitaiStream io, string srcPath)
            : base("not equal, expected " + ByteArrayToHex(expected) + ", but got " + ByteArrayToHex(actual), io, srcPath)
        {
            this.expected = expected;
            this.actual = actual;
        }

        public ValidationNotEqualError(Object expected, Object actual, KaitaiStream io, string srcPath)
            : base("not equal, expected " + expected + ", but got " + actual, io, srcPath)
        {
            this.expected = expected;
            this.actual = actual;
        }

        protected Object expected;
        protected Object actual;
    }

    public class ValidationLessThanError : ValidationFailedError
    {
        public ValidationLessThanError(byte[] min, byte[] actual, KaitaiStream io, string srcPath)
            : base("not in range, min " + ByteArrayToHex(min) + ", but got " + ByteArrayToHex(actual), io, srcPath)
        {
            this.min = min;
            this.actual = actual;
        }

        public ValidationLessThanError(Object min, Object actual, KaitaiStream io, string srcPath)
            : base("not in range, min " + min + ", but got " + actual, io, srcPath)
        {
            this.min = min;
            this.actual = actual;
        }

        protected Object min;
        protected Object actual;
    }

    public class ValidationGreaterThanError : ValidationFailedError
    {
        public ValidationGreaterThanError(byte[] max, byte[] actual, KaitaiStream io, string srcPath)
            : base("not in range, max " + ByteArrayToHex(max) + ", but got " + ByteArrayToHex(actual), io, srcPath)
        {
            this.max = max;
            this.actual = actual;
        }

        public ValidationGreaterThanError(Object max, Object actual, KaitaiStream io, string srcPath)
            : base("not in range, max " + max + ", but got " + actual, io, srcPath)
        {
            this.max = max;
            this.actual = actual;
        }

        protected Object max;
        protected Object actual;
    }

    public class ValidationNotAnyOfError : ValidationFailedError
    {
        public ValidationNotAnyOfError(Object actual, KaitaiStream io, string srcPath)
            : base("not any of the list, got " + actual, io, srcPath)
        {
            this.actual = actual;
        }

        protected Object actual;
    }

    public class ValidationNotInEnumError : ValidationFailedError
    {
        public ValidationNotInEnumError(Object actual, KaitaiStream io, string srcPath)
            : base("not in the enum, got " + actual, io, srcPath)
        {
            this.actual = actual;
        }

        protected Object actual;
    }

    public class ValidationExprError : ValidationFailedError
    {
        public ValidationExprError(Object actual, KaitaiStream io, string srcPath)
            : base("not matching the expression, got " + actual, io, srcPath)
        {
            this.actual = actual;
        }

        protected Object actual;
    }
}
