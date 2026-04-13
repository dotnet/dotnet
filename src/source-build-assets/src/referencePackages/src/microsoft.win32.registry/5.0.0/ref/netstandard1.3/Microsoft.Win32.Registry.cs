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
[assembly: System.Reflection.AssemblyTitle("Microsoft.Win32.Registry")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Win32.Registry")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Win32.Registry")]
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
namespace Microsoft.Win32
{
    public static partial class Registry
    {
        public static readonly RegistryKey ClassesRoot;
        public static readonly RegistryKey CurrentConfig;
        public static readonly RegistryKey CurrentUser;
        public static readonly RegistryKey LocalMachine;
        public static readonly RegistryKey PerformanceData;
        public static readonly RegistryKey Users;
        public static object GetValue(string keyName, string valueName, object defaultValue) { throw null; }

        public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind) { }

        public static void SetValue(string keyName, string valueName, object value) { }
    }

    public enum RegistryHive
    {
        ClassesRoot = int.MinValue,
        CurrentUser = -2147483647,
        LocalMachine = -2147483646,
        Users = -2147483645,
        PerformanceData = -2147483644,
        CurrentConfig = -2147483643
    }

    public sealed partial class RegistryKey : System.IDisposable
    {
        internal RegistryKey() { }

        public SafeHandles.SafeRegistryHandle Handle { get { throw null; } }

        public string Name { get { throw null; } }

        public int SubKeyCount { get { throw null; } }

        public int ValueCount { get { throw null; } }

        public RegistryView View { get { throw null; } }

        public RegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options) { throw null; }

        public RegistryKey CreateSubKey(string subkey, bool writable) { throw null; }

        public RegistryKey CreateSubKey(string subkey) { throw null; }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey) { }

        public void DeleteSubKey(string subkey) { }

        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey) { }

        public void DeleteSubKeyTree(string subkey) { }

        public void DeleteValue(string name, bool throwOnMissingValue) { }

        public void DeleteValue(string name) { }

        public void Dispose() { }

        public void Flush() { }

        public static RegistryKey FromHandle(SafeHandles.SafeRegistryHandle handle, RegistryView view) { throw null; }

        public static RegistryKey FromHandle(SafeHandles.SafeRegistryHandle handle) { throw null; }

        public string[] GetSubKeyNames() { throw null; }

        public object GetValue(string name, object defaultValue, RegistryValueOptions options) { throw null; }

        public object GetValue(string name, object defaultValue) { throw null; }

        public object GetValue(string name) { throw null; }

        public RegistryValueKind GetValueKind(string name) { throw null; }

        public string[] GetValueNames() { throw null; }

        public static RegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view) { throw null; }

        public RegistryKey OpenSubKey(string name, bool writable) { throw null; }

        public RegistryKey OpenSubKey(string name, System.Security.AccessControl.RegistryRights rights) { throw null; }

        public RegistryKey OpenSubKey(string name) { throw null; }

        public void SetValue(string name, object value, RegistryValueKind valueKind) { }

        public void SetValue(string name, object value) { }

        public override string ToString() { throw null; }
    }

    [System.Flags]
    public enum RegistryOptions
    {
        None = 0,
        Volatile = 1
    }

    public enum RegistryValueKind
    {
        None = -1,
        Unknown = 0,
        String = 1,
        ExpandString = 2,
        Binary = 3,
        DWord = 4,
        MultiString = 7,
        QWord = 11
    }

    [System.Flags]
    public enum RegistryValueOptions
    {
        None = 0,
        DoNotExpandEnvironmentNames = 1
    }

    public enum RegistryView
    {
        Default = 0,
        Registry64 = 256,
        Registry32 = 512
    }
}

namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeRegistryHandle : System.Runtime.InteropServices.SafeHandle
    {
        public SafeRegistryHandle(System.IntPtr preexistingHandle, bool ownsHandle) : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.Security.AccessControl
{
    [Flags]
    public enum RegistryRights
    {
        QueryValues = 1,
        SetValue = 2,
        CreateSubKey = 4,
        EnumerateSubKeys = 8,
        Notify = 16,
        CreateLink = 32,
        Delete = 65536,
        ReadPermissions = 131072,
        WriteKey = 131078,
        ExecuteKey = 131097,
        ReadKey = 131097,
        ChangePermissions = 262144,
        TakeOwnership = 524288,
        FullControl = 983103
    }
}