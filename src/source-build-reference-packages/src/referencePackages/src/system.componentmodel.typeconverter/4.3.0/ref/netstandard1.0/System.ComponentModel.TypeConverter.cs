// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyDescription("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ComponentModel.TypeConverter")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.23123.00")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.23123.00 built by: PROJECTKREL")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("", "")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.ComponentModel
{
    public partial class ArrayConverter : CollectionConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public abstract partial class BaseNumberConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type t) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class BooleanConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }
    }

    public partial class ByteConverter : BaseNumberConverter
    {
    }

    public partial class CharConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class CollectionConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DateTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DateTimeOffsetConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DecimalConverter : BaseNumberConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class DoubleConverter : BaseNumberConverter
    {
    }

    public partial class EnumConverter : TypeConverter
    {
        public EnumConverter(Type type) { }

        protected Type EnumType { get { throw null; } }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class GuidConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class Int16Converter : BaseNumberConverter
    {
    }

    public partial class Int32Converter : BaseNumberConverter
    {
    }

    public partial class Int64Converter : BaseNumberConverter
    {
    }

    public partial interface ITypeDescriptorContext : IServiceProvider
    {
        IContainer Container { get; }

        object Instance { get; }

        PropertyDescriptor PropertyDescriptor { get; }

        void OnComponentChanged();
        bool OnComponentChanging();
    }

    public partial class MultilineStringConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class NullableConverter : TypeConverter
    {
        public NullableConverter(Type type) { }

        public Type NullableType { get { throw null; } }

        public Type UnderlyingType { get { throw null; } }

        public TypeConverter UnderlyingTypeConverter { get { throw null; } }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public abstract partial class PropertyDescriptor
    {
        internal PropertyDescriptor() { }
    }

    public partial class SByteConverter : BaseNumberConverter
    {
    }

    public partial class SingleConverter : BaseNumberConverter
    {
    }

    public partial class StringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }
    }

    public partial class TimeSpanConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class TypeConverter
    {
        public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public bool CanConvertFrom(Type sourceType) { throw null; }

        public virtual bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public bool CanConvertTo(Type destinationType) { throw null; }

        public virtual object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public object ConvertFrom(object value) { throw null; }

        public object ConvertFromInvariantString(string text) { throw null; }

        public object ConvertFromString(ITypeDescriptorContext context, Globalization.CultureInfo culture, string text) { throw null; }

        public object ConvertFromString(string text) { throw null; }

        public virtual object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public object ConvertTo(object value, Type destinationType) { throw null; }

        public string ConvertToInvariantString(object value) { throw null; }

        public string ConvertToString(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public string ConvertToString(object value) { throw null; }

        protected Exception GetConvertFromException(object value) { throw null; }

        protected Exception GetConvertToException(object value, Type destinationType) { throw null; }
    }

    public sealed partial class TypeDescriptor
    {
        internal TypeDescriptor() { }

        public static TypeConverter GetConverter(Type type) { throw null; }
    }

    public abstract partial class TypeListConverter : TypeConverter
    {
        protected TypeListConverter(Type[] types) { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class UInt16Converter : BaseNumberConverter
    {
    }

    public partial class UInt32Converter : BaseNumberConverter
    {
    }

    public partial class UInt64Converter : BaseNumberConverter
    {
    }
}