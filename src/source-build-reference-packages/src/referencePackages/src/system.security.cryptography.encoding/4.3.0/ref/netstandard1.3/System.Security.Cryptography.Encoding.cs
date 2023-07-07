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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Encoding")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.Encoding")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Encoding")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography
{
    public partial class AsnEncodedData
    {
        protected AsnEncodedData() { }

        public AsnEncodedData(byte[] rawData) { }

        public AsnEncodedData(AsnEncodedData asnEncodedData) { }

        public AsnEncodedData(Oid oid, byte[] rawData) { }

        public AsnEncodedData(string oid, byte[] rawData) { }

        public Oid Oid { get { throw null; } set { } }

        public byte[] RawData { get { throw null; } set { } }

        public virtual void CopyFrom(AsnEncodedData asnEncodedData) { }

        public virtual string Format(bool multiLine) { throw null; }
    }

    public sealed partial class AsnEncodedDataCollection : Collections.ICollection, Collections.IEnumerable
    {
        public AsnEncodedDataCollection() { }

        public AsnEncodedDataCollection(AsnEncodedData asnEncodedData) { }

        public int Count { get { throw null; } }

        public AsnEncodedData this[int index] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public int Add(AsnEncodedData asnEncodedData) { throw null; }

        public void CopyTo(AsnEncodedData[] array, int index) { }

        public AsnEncodedDataEnumerator GetEnumerator() { throw null; }

        public void Remove(AsnEncodedData asnEncodedData) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class AsnEncodedDataEnumerator : Collections.IEnumerator
    {
        internal AsnEncodedDataEnumerator() { }

        public AsnEncodedData Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public sealed partial class Oid
    {
        public Oid(Oid oid) { }

        public Oid(string value, string friendlyName) { }

        public Oid(string oid) { }

        public string FriendlyName { get { throw null; } set { } }

        public string Value { get { throw null; } set { } }

        public static Oid FromFriendlyName(string friendlyName, OidGroup group) { throw null; }

        public static Oid FromOidValue(string oidValue, OidGroup group) { throw null; }
    }

    public sealed partial class OidCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public Oid this[int index] { get { throw null; } }

        public Oid this[string oid] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public int Add(Oid oid) { throw null; }

        public void CopyTo(Oid[] array, int index) { }

        public OidEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class OidEnumerator : Collections.IEnumerator
    {
        internal OidEnumerator() { }

        public Oid Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public enum OidGroup
    {
        All = 0,
        HashAlgorithm = 1,
        EncryptionAlgorithm = 2,
        PublicKeyAlgorithm = 3,
        SignatureAlgorithm = 4,
        Attribute = 5,
        ExtensionOrAttribute = 6,
        EnhancedKeyUsage = 7,
        Policy = 8,
        Template = 9,
        KeyDerivationFunction = 10
    }
}