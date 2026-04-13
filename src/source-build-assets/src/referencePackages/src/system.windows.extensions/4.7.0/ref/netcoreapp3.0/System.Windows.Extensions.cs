// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Windows.Extensions")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Windows.Extensions")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Windows.Extensions")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Drawing
{
    public partial class FontConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override object CreateInstance(ComponentModel.ITypeDescriptorContext context, Collections.IDictionary propertyValues) { throw null; }

        public override bool GetCreateInstanceSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }

        public override ComponentModel.PropertyDescriptorCollection GetProperties(ComponentModel.ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }

        public sealed partial class FontNameConverter : ComponentModel.TypeConverter, IDisposable
        {
            public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

            public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

            public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext context) { throw null; }

            public override bool GetStandardValuesExclusive(ComponentModel.ITypeDescriptorContext context) { throw null; }

            public override bool GetStandardValuesSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }

            void IDisposable.Dispose() { }
        }

        public partial class FontUnitConverter : ComponentModel.EnumConverter
        {
            public FontUnitConverter() : base(default!) { }

            public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext context) { throw null; }
        }
    }

    public partial class IconConverter : ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }
    }

    public partial class ImageConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override ComponentModel.PropertyDescriptorCollection GetProperties(ComponentModel.ITypeDescriptorContext context, object value, Attribute[] attributes) { throw null; }

        public override bool GetPropertiesSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }
    }

    public partial class ImageFormatConverter : ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override StandardValuesCollection GetStandardValues(ComponentModel.ITypeDescriptorContext context) { throw null; }

        public override bool GetStandardValuesSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }
    }
}

namespace System.Drawing.Printing
{
    public partial class MarginsConverter : ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ComponentModel.ITypeDescriptorContext context, Type sourceType) { throw null; }

        public override bool CanConvertTo(ComponentModel.ITypeDescriptorContext context, Type destinationType) { throw null; }

        public override object ConvertFrom(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value) { throw null; }

        public override object ConvertTo(ComponentModel.ITypeDescriptorContext context, Globalization.CultureInfo culture, object value, Type destinationType) { throw null; }

        public override object CreateInstance(ComponentModel.ITypeDescriptorContext context, Collections.IDictionary propertyValues) { throw null; }

        public override bool GetCreateInstanceSupported(ComponentModel.ITypeDescriptorContext context) { throw null; }
    }
}

namespace System.Media
{
    public partial class SoundPlayer : ComponentModel.Component, Runtime.Serialization.ISerializable
    {
        public SoundPlayer() { }

        public SoundPlayer(IO.Stream stream) { }

        protected SoundPlayer(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext context) { }

        public SoundPlayer(string soundLocation) { }

        public bool IsLoadCompleted { get { throw null; } }

        public int LoadTimeout { get { throw null; } set { } }

        public string SoundLocation { get { throw null; } set { } }

        public IO.Stream Stream { get { throw null; } set { } }

        public object Tag { get { throw null; } set { } }

        public event ComponentModel.AsyncCompletedEventHandler LoadCompleted { add { } remove { } }

        public event EventHandler SoundLocationChanged { add { } remove { } }

        public event EventHandler StreamChanged { add { } remove { } }

        public void Load() { }

        public void LoadAsync() { }

        protected virtual void OnLoadCompleted(ComponentModel.AsyncCompletedEventArgs e) { }

        protected virtual void OnSoundLocationChanged(EventArgs e) { }

        protected virtual void OnStreamChanged(EventArgs e) { }

        public void Play() { }

        public void PlayLooping() { }

        public void PlaySync() { }

        public void Stop() { }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class SystemSound
    {
        internal SystemSound() { }

        public void Play() { }
    }

    public static partial class SystemSounds
    {
        public static SystemSound Asterisk { get { throw null; } }

        public static SystemSound Beep { get { throw null; } }

        public static SystemSound Exclamation { get { throw null; } }

        public static SystemSound Hand { get { throw null; } }

        public static SystemSound Question { get { throw null; } }
    }
}

namespace System.Security.Cryptography.X509Certificates
{
    public sealed partial class X509Certificate2UI
    {
        public static void DisplayCertificate(X509Certificate2 certificate, IntPtr hwndParent) { }

        public static void DisplayCertificate(X509Certificate2 certificate) { }

        public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string title, string message, X509SelectionFlag selectionFlag, IntPtr hwndParent) { throw null; }

        public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string title, string message, X509SelectionFlag selectionFlag) { throw null; }
    }

    public enum X509SelectionFlag
    {
        SingleSelection = 0,
        MultiSelection = 1
    }
}

namespace System.Xaml.Permissions
{
    public partial class XamlAccessLevel
    {
        internal XamlAccessLevel() { }

        public Reflection.AssemblyName AssemblyAccessToAssemblyName { get { throw null; } }

        public string PrivateAccessToTypeName { get { throw null; } }

        public static XamlAccessLevel AssemblyAccessTo(Reflection.Assembly assembly) { throw null; }

        public static XamlAccessLevel AssemblyAccessTo(Reflection.AssemblyName assemblyName) { throw null; }

        public static XamlAccessLevel PrivateAccessTo(string assemblyQualifiedTypeName) { throw null; }

        public static XamlAccessLevel PrivateAccessTo(Type type) { throw null; }
    }
}