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
[assembly: System.Reflection.AssemblyTitle("System.Runtime.Serialization.Formatters")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.Serialization.Formatters")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.Serialization.Formatters")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed partial class NonSerializedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
    public sealed partial class SerializableAttribute : Attribute
    {
    }
}

namespace System.Runtime.Serialization
{
    public partial interface IDeserializationCallback
    {
        void OnDeserialization(object sender);
    }

    [CLSCompliant(false)]
    public partial interface IFormatterConverter
    {
        object Convert(object value, Type type);
        object Convert(object value, TypeCode typeCode);
        bool ToBoolean(object value);
        byte ToByte(object value);
        char ToChar(object value);
        DateTime ToDateTime(object value);
        decimal ToDecimal(object value);
        double ToDouble(object value);
        short ToInt16(object value);
        int ToInt32(object value);
        long ToInt64(object value);
        [CLSCompliant(false)]
        sbyte ToSByte(object value);
        float ToSingle(object value);
        string ToString(object value);
        [CLSCompliant(false)]
        ushort ToUInt16(object value);
        [CLSCompliant(false)]
        uint ToUInt32(object value);
        [CLSCompliant(false)]
        ulong ToUInt64(object value);
    }

    public partial interface ISerializable
    {
        void GetObjectData(SerializationInfo info, StreamingContext context);
    }

    public partial struct SerializationEntry
    {
        public string Name { get { throw null; } }

        public Type ObjectType { get { throw null; } }

        public object Value { get { throw null; } }
    }

    public sealed partial class SerializationInfo
    {
        [CLSCompliant(false)]
        public SerializationInfo(Type type, IFormatterConverter converter) { }

        public string AssemblyName { get { throw null; } set { } }

        public string FullTypeName { get { throw null; } set { } }

        public int MemberCount { get { throw null; } }

        public Type ObjectType { get { throw null; } }

        public void AddValue(string name, bool value) { }

        public void AddValue(string name, byte value) { }

        public void AddValue(string name, char value) { }

        public void AddValue(string name, DateTime value) { }

        public void AddValue(string name, decimal value) { }

        public void AddValue(string name, double value) { }

        public void AddValue(string name, short value) { }

        public void AddValue(string name, int value) { }

        public void AddValue(string name, long value) { }

        public void AddValue(string name, object value, Type type) { }

        public void AddValue(string name, object value) { }

        [CLSCompliant(false)]
        public void AddValue(string name, sbyte value) { }

        public void AddValue(string name, float value) { }

        [CLSCompliant(false)]
        public void AddValue(string name, ushort value) { }

        [CLSCompliant(false)]
        public void AddValue(string name, uint value) { }

        [CLSCompliant(false)]
        public void AddValue(string name, ulong value) { }

        public bool GetBoolean(string name) { throw null; }

        public byte GetByte(string name) { throw null; }

        public char GetChar(string name) { throw null; }

        public DateTime GetDateTime(string name) { throw null; }

        public decimal GetDecimal(string name) { throw null; }

        public double GetDouble(string name) { throw null; }

        public SerializationInfoEnumerator GetEnumerator() { throw null; }

        public short GetInt16(string name) { throw null; }

        public int GetInt32(string name) { throw null; }

        public long GetInt64(string name) { throw null; }

        [CLSCompliant(false)]
        public sbyte GetSByte(string name) { throw null; }

        public float GetSingle(string name) { throw null; }

        public string GetString(string name) { throw null; }

        [CLSCompliant(false)]
        public ushort GetUInt16(string name) { throw null; }

        [CLSCompliant(false)]
        public uint GetUInt32(string name) { throw null; }

        [CLSCompliant(false)]
        public ulong GetUInt64(string name) { throw null; }

        public object GetValue(string name, Type type) { throw null; }

        public void SetType(Type type) { }
    }

    public sealed partial class SerializationInfoEnumerator : Collections.IEnumerator
    {
        internal SerializationInfoEnumerator() { }

        public SerializationEntry Current { get { throw null; } }

        public string Name { get { throw null; } }

        public Type ObjectType { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public object Value { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }
}