#if NET452

// FormattableString & Co are available in NetFx 4.6+, but not in NetFx 4.5 or NetStandard 1.1

#pragma warning disable SA1402  // File may only contain a single class
#pragma warning disable SA1403  // File may only contain a single namespace
#pragma warning disable SA1649  // File name must match first type name

namespace System.Runtime.CompilerServices
{
    using System;

    internal class FormattableStringFactory
    {
        public static FormattableString Create(string format, params object[] args)
        {
            return new FormattableString(format, args);
        }
    }
}

namespace System
{
    using System.Globalization;

    internal class FormattableString
    {
        private readonly string format;
        private readonly object[] args;

        public FormattableString(string format, object[] args)
        {
            this.format = format;
            this.args = args;
        }

        public static string Invariant(FormattableString formattableString)
        {
            string str = formattableString.ToString(CultureInfo.InvariantCulture);
            return str;
        }

        public override string ToString()
        {
            string str = String.Format(CultureInfo.InvariantCulture, this.format, this.args);
            return str;
        }

        public string ToString(IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                throw new ArgumentNullException(nameof(formatProvider));
            }

            string str = String.Format(formatProvider, this.format, this.args);
            return str;
        }
    }
}

#pragma warning restore SA1649  // File name must match first type name
#pragma warning restore SA1403  // File may only contain a single namespace
#pragma warning restore SA1402  // File may only contain a single class

#endif