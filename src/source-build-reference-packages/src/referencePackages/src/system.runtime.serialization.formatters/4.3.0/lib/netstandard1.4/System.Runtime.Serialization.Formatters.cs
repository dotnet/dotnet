// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Runtime.Serialization.Formatters")]
[assembly: AssemblyDescription("System.Runtime.Serialization.Formatters")]
[assembly: AssemblyDefaultAlias("System.Runtime.Serialization.Formatters")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Field, Inherited=false)]
    public sealed partial class NonSerializedAttribute : System.Attribute
    {
        public NonSerializedAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Struct, Inherited=false)]
    public sealed partial class SerializableAttribute : System.Attribute
    {
        public SerializableAttribute() { }
    }
}
namespace System.Runtime.Serialization
{
    public partial interface IDeserializationCallback
    {
        void OnDeserialization(object sender);
    }
    [System.CLSCompliantAttribute(false)]
    public partial interface IFormatterConverter
    {
        object Convert(object value, System.Type type);
        object Convert(object value, System.TypeCode typeCode);
        bool ToBoolean(object value);
        byte ToByte(object value);
        char ToChar(object value);
        System.DateTime ToDateTime(object value);
        decimal ToDecimal(object value);
        double ToDouble(object value);
        short ToInt16(object value);
        int ToInt32(object value);
        long ToInt64(object value);
        [System.CLSCompliantAttribute(false)]
        sbyte ToSByte(object value);
        float ToSingle(object value);
        string ToString(object value);
        [System.CLSCompliantAttribute(false)]
        ushort ToUInt16(object value);
        [System.CLSCompliantAttribute(false)]
        uint ToUInt32(object value);
        [System.CLSCompliantAttribute(false)]
        ulong ToUInt64(object value);
    }
    public partial interface ISerializable
    {
        void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context);
    }
    public partial struct SerializationEntry
    {
        private object _dummy;
        public string Name { get { throw null; } }
        public System.Type ObjectType { get { throw null; } }
        public object Value { get { throw null; } }
    }
    public sealed partial class SerializationInfo
    {
        [System.CLSCompliantAttribute(false)]
        public SerializationInfo(System.Type type, System.Runtime.Serialization.IFormatterConverter converter) { }
        public string AssemblyName { get { throw null; } set { } }
        public string FullTypeName { get { throw null; } set { } }
        public int MemberCount { get { throw null; } }
        public System.Type ObjectType { get { throw null; } }
        public void AddValue(string name, bool value) { }
        public void AddValue(string name, byte value) { }
        public void AddValue(string name, char value) { }
        public void AddValue(string name, System.DateTime value) { }
        public void AddValue(string name, decimal value) { }
        public void AddValue(string name, double value) { }
        public void AddValue(string name, short value) { }
        public void AddValue(string name, int value) { }
        public void AddValue(string name, long value) { }
        public void AddValue(string name, object value) { }
        public void AddValue(string name, object value, System.Type type) { }
        [System.CLSCompliantAttribute(false)]
        public void AddValue(string name, sbyte value) { }
        public void AddValue(string name, float value) { }
        [System.CLSCompliantAttribute(false)]
        public void AddValue(string name, ushort value) { }
        [System.CLSCompliantAttribute(false)]
        public void AddValue(string name, uint value) { }
        [System.CLSCompliantAttribute(false)]
        public void AddValue(string name, ulong value) { }
        public bool GetBoolean(string name) { throw null; }
        public byte GetByte(string name) { throw null; }
        public char GetChar(string name) { throw null; }
        public System.DateTime GetDateTime(string name) { throw null; }
        public decimal GetDecimal(string name) { throw null; }
        public double GetDouble(string name) { throw null; }
        public System.Runtime.Serialization.SerializationInfoEnumerator GetEnumerator() { throw null; }
        public short GetInt16(string name) { throw null; }
        public int GetInt32(string name) { throw null; }
        public long GetInt64(string name) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public sbyte GetSByte(string name) { throw null; }
        public float GetSingle(string name) { throw null; }
        public string GetString(string name) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public ushort GetUInt16(string name) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public uint GetUInt32(string name) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public ulong GetUInt64(string name) { throw null; }
        public object GetValue(string name, System.Type type) { throw null; }
        public void SetType(System.Type type) { }
    }
    public sealed partial class SerializationInfoEnumerator : System.Collections.IEnumerator
    {
        internal SerializationInfoEnumerator() { }
        public System.Runtime.Serialization.SerializationEntry Current { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Type ObjectType { get { throw null; } }
        object System.Collections.IEnumerator.Current { get { throw null; } }
        public object Value { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
}
