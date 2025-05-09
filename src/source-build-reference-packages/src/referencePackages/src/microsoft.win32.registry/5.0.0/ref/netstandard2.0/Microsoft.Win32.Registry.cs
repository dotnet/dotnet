// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Win32.Registry")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Win32.Registry")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Win32.Registry")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
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
        public static object? GetValue(string keyName, string? valueName, object? defaultValue) { throw null; }
        public static void SetValue(string keyName, string? valueName, object value, RegistryValueKind valueKind) { }
        public static void SetValue(string keyName, string? valueName, object value) { }
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

    public sealed partial class RegistryKey : System.MarshalByRefObject, System.IDisposable
    {
        internal RegistryKey() { }
        public SafeHandles.SafeRegistryHandle Handle { get { throw null; } }
        public string Name { get { throw null; } }
        public int SubKeyCount { get { throw null; } }
        public int ValueCount { get { throw null; } }
        public RegistryView View { get { throw null; } }

        public void Close() { }
        public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, System.Security.AccessControl.RegistrySecurity? registrySecurity) { throw null; }
        public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions) { throw null; }
        public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistrySecurity? registrySecurity) { throw null; }
        public RegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck) { throw null; }
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
        public System.Security.AccessControl.RegistrySecurity GetAccessControl() { throw null; }
        public System.Security.AccessControl.RegistrySecurity GetAccessControl(System.Security.AccessControl.AccessControlSections includeSections) { throw null; }
        public string[] GetSubKeyNames() { throw null; }
        public object? GetValue(string? name, object? defaultValue, RegistryValueOptions options) { throw null; }
        public object? GetValue(string? name, object? defaultValue) { throw null; }
        public object? GetValue(string? name) { throw null; }
        public RegistryValueKind GetValueKind(string? name) { throw null; }
        public string[] GetValueNames() { throw null; }
        public static RegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view) { throw null; }
        public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view) { throw null; }
        public static RegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName) { throw null; }
        public RegistryKey? OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, System.Security.AccessControl.RegistryRights rights) { throw null; }
        public RegistryKey? OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck) { throw null; }
        public RegistryKey? OpenSubKey(string name, bool writable) { throw null; }
        public RegistryKey? OpenSubKey(string name, System.Security.AccessControl.RegistryRights rights) { throw null; }
        public RegistryKey? OpenSubKey(string name) { throw null; }
        public void SetAccessControl(System.Security.AccessControl.RegistrySecurity registrySecurity) { }
        public void SetValue(string? name, object value, RegistryValueKind valueKind) { }
        public void SetValue(string? name, object value) { }
        public override string ToString() { throw null; }
    }

    public enum RegistryKeyPermissionCheck
    {
        Default = 0,
        ReadSubTree = 1,
        ReadWriteSubTree = 2
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
    public sealed partial class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeRegistryHandle(System.IntPtr preexistingHandle, bool ownsHandle) : base(default) { }
        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.Security.AccessControl
{
    public sealed partial class RegistryAccessRule : AccessRule
    {
        public RegistryAccessRule(Principal.IdentityReference identity, RegistryRights registryRights, AccessControlType type) : base(default!, default, default, default, default, default) { }
        public RegistryAccessRule(Principal.IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }
        public RegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type) : base(default!, default, default, default, default, default) { }
        public RegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }
        public RegistryRights RegistryRights { get { throw null; } }
    }

    public sealed partial class RegistryAuditRule : AuditRule
    {
        public RegistryAuditRule(Principal.IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }
        public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }
        public RegistryRights RegistryRights { get { throw null; } }
    }

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

    public sealed partial class RegistrySecurity : NativeObjectSecurity
    {
        public RegistrySecurity() : base(default, default) { }
        public override Type AccessRightType { get { throw null; } }
        public override Type AccessRuleType { get { throw null; } }
        public override Type AuditRuleType { get { throw null; } }

        public override AccessRule AccessRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) { throw null; }
        public void AddAccessRule(RegistryAccessRule rule) { }
        public void AddAuditRule(RegistryAuditRule rule) { }
        public override AuditRule AuditRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) { throw null; }
        public bool RemoveAccessRule(RegistryAccessRule rule) { throw null; }
        public void RemoveAccessRuleAll(RegistryAccessRule rule) { }
        public void RemoveAccessRuleSpecific(RegistryAccessRule rule) { }
        public bool RemoveAuditRule(RegistryAuditRule rule) { throw null; }
        public void RemoveAuditRuleAll(RegistryAuditRule rule) { }
        public void RemoveAuditRuleSpecific(RegistryAuditRule rule) { }
        public void ResetAccessRule(RegistryAccessRule rule) { }
        public void SetAccessRule(RegistryAccessRule rule) { }
        public void SetAuditRule(RegistryAuditRule rule) { }
    }
}