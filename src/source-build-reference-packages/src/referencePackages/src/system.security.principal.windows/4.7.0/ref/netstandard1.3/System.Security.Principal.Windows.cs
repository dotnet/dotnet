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
[assembly: System.Reflection.AssemblyTitle("System.Security.Principal.Windows")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Principal.Windows")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Principal.Windows")]
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
namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeAccessTokenHandle : System.Runtime.InteropServices.SafeHandle
    {
        public SafeAccessTokenHandle(System.IntPtr handle) : base(default, default) { }

        public static SafeAccessTokenHandle InvalidHandle { get { throw null; } }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.Security.Principal
{
    public sealed partial class IdentityNotMappedException : Exception
    {
        public IdentityNotMappedException() { }

        public IdentityNotMappedException(string message, Exception inner) { }

        public IdentityNotMappedException(string message) { }

        public IdentityReferenceCollection UnmappedIdentities { get { throw null; } }
    }

    public abstract partial class IdentityReference
    {
        internal IdentityReference() { }

        public abstract string Value { get; }

        public abstract override bool Equals(object o);
        public abstract override int GetHashCode();
        public abstract bool IsValidTargetType(Type targetType);
        public static bool operator ==(IdentityReference left, IdentityReference right) { throw null; }

        public static bool operator !=(IdentityReference left, IdentityReference right) { throw null; }

        public abstract override string ToString();
        public abstract IdentityReference Translate(Type targetType);
    }

    public partial class IdentityReferenceCollection : Collections.Generic.ICollection<IdentityReference>, Collections.Generic.IEnumerable<IdentityReference>, Collections.IEnumerable
    {
        public IdentityReferenceCollection() { }

        public IdentityReferenceCollection(int capacity) { }

        public int Count { get { throw null; } }

        public IdentityReference this[int index] { get { throw null; } set { } }

        bool Collections.Generic.ICollection<IdentityReference>.IsReadOnly { get { throw null; } }

        public void Add(IdentityReference identity) { }

        public void Clear() { }

        public bool Contains(IdentityReference identity) { throw null; }

        public void CopyTo(IdentityReference[] array, int offset) { }

        public Collections.Generic.IEnumerator<IdentityReference> GetEnumerator() { throw null; }

        public bool Remove(IdentityReference identity) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public IdentityReferenceCollection Translate(Type targetType, bool forceSuccess) { throw null; }

        public IdentityReferenceCollection Translate(Type targetType) { throw null; }
    }

    public sealed partial class NTAccount : IdentityReference
    {
        public NTAccount(string domainName, string accountName) { }

        public NTAccount(string name) { }

        public override string Value { get { throw null; } }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override bool IsValidTargetType(Type targetType) { throw null; }

        public static bool operator ==(NTAccount left, NTAccount right) { throw null; }

        public static bool operator !=(NTAccount left, NTAccount right) { throw null; }

        public override string ToString() { throw null; }

        public override IdentityReference Translate(Type targetType) { throw null; }
    }

    public sealed partial class SecurityIdentifier : IdentityReference, IComparable<SecurityIdentifier>
    {
        public static readonly int MaxBinaryLength;
        public static readonly int MinBinaryLength;
        public SecurityIdentifier(byte[] binaryForm, int offset) { }

        public SecurityIdentifier(IntPtr binaryForm) { }

        public SecurityIdentifier(WellKnownSidType sidType, SecurityIdentifier domainSid) { }

        public SecurityIdentifier(string sddlForm) { }

        public SecurityIdentifier AccountDomainSid { get { throw null; } }

        public int BinaryLength { get { throw null; } }

        public override string Value { get { throw null; } }

        public int CompareTo(SecurityIdentifier sid) { throw null; }

        public override bool Equals(object o) { throw null; }

        public bool Equals(SecurityIdentifier sid) { throw null; }

        public void GetBinaryForm(byte[] binaryForm, int offset) { }

        public override int GetHashCode() { throw null; }

        public bool IsAccountSid() { throw null; }

        public bool IsEqualDomainSid(SecurityIdentifier sid) { throw null; }

        public override bool IsValidTargetType(Type targetType) { throw null; }

        public bool IsWellKnown(WellKnownSidType type) { throw null; }

        public static bool operator ==(SecurityIdentifier left, SecurityIdentifier right) { throw null; }

        public static bool operator !=(SecurityIdentifier left, SecurityIdentifier right) { throw null; }

        public override string ToString() { throw null; }

        public override IdentityReference Translate(Type targetType) { throw null; }
    }

    [Flags]
    public enum TokenAccessLevels
    {
        AssignPrimary = 1,
        Duplicate = 2,
        Impersonate = 4,
        Query = 8,
        QuerySource = 16,
        AdjustPrivileges = 32,
        AdjustGroups = 64,
        AdjustDefault = 128,
        AdjustSessionId = 256,
        Read = 131080,
        Write = 131296,
        AllAccess = 983551,
        MaximumAllowed = 33554432
    }

    public enum WellKnownSidType
    {
        NullSid = 0,
        WorldSid = 1,
        LocalSid = 2,
        CreatorOwnerSid = 3,
        CreatorGroupSid = 4,
        CreatorOwnerServerSid = 5,
        CreatorGroupServerSid = 6,
        NTAuthoritySid = 7,
        DialupSid = 8,
        NetworkSid = 9,
        BatchSid = 10,
        InteractiveSid = 11,
        ServiceSid = 12,
        AnonymousSid = 13,
        ProxySid = 14,
        EnterpriseControllersSid = 15,
        SelfSid = 16,
        AuthenticatedUserSid = 17,
        RestrictedCodeSid = 18,
        TerminalServerSid = 19,
        RemoteLogonIdSid = 20,
        LogonIdsSid = 21,
        LocalSystemSid = 22,
        LocalServiceSid = 23,
        NetworkServiceSid = 24,
        BuiltinDomainSid = 25,
        BuiltinAdministratorsSid = 26,
        BuiltinUsersSid = 27,
        BuiltinGuestsSid = 28,
        BuiltinPowerUsersSid = 29,
        BuiltinAccountOperatorsSid = 30,
        BuiltinSystemOperatorsSid = 31,
        BuiltinPrintOperatorsSid = 32,
        BuiltinBackupOperatorsSid = 33,
        BuiltinReplicatorSid = 34,
        BuiltinPreWindows2000CompatibleAccessSid = 35,
        BuiltinRemoteDesktopUsersSid = 36,
        BuiltinNetworkConfigurationOperatorsSid = 37,
        AccountAdministratorSid = 38,
        AccountGuestSid = 39,
        AccountKrbtgtSid = 40,
        AccountDomainAdminsSid = 41,
        AccountDomainUsersSid = 42,
        AccountDomainGuestsSid = 43,
        AccountComputersSid = 44,
        AccountControllersSid = 45,
        AccountCertAdminsSid = 46,
        AccountSchemaAdminsSid = 47,
        AccountEnterpriseAdminsSid = 48,
        AccountPolicyAdminsSid = 49,
        AccountRasAndIasServersSid = 50,
        NtlmAuthenticationSid = 51,
        DigestAuthenticationSid = 52,
        SChannelAuthenticationSid = 53,
        ThisOrganizationSid = 54,
        OtherOrganizationSid = 55,
        BuiltinIncomingForestTrustBuildersSid = 56,
        BuiltinPerformanceMonitoringUsersSid = 57,
        BuiltinPerformanceLoggingUsersSid = 58,
        BuiltinAuthorizationAccessSid = 59,
        MaxDefined = 60,
        WinBuiltinTerminalServerLicenseServersSid = 60
    }

    public enum WindowsBuiltInRole
    {
        Administrator = 544,
        User = 545,
        Guest = 546,
        PowerUser = 547,
        AccountOperator = 548,
        SystemOperator = 549,
        PrintOperator = 550,
        BackupOperator = 551,
        Replicator = 552
    }

    public partial class WindowsIdentity : Claims.ClaimsIdentity, IDisposable
    {
        public new const string DefaultIssuer = "AD AUTHORITY";
        public WindowsIdentity(IntPtr userToken, string type) { }

        public WindowsIdentity(IntPtr userToken) { }

        public WindowsIdentity(string sUserPrincipalName) { }

        public Microsoft.Win32.SafeHandles.SafeAccessTokenHandle AccessToken { get { throw null; } }

        public sealed override string AuthenticationType { get { throw null; } }

        public override Collections.Generic.IEnumerable<Claims.Claim> Claims { get { throw null; } }

        public IdentityReferenceCollection Groups { get { throw null; } }

        public TokenImpersonationLevel ImpersonationLevel { get { throw null; } }

        public virtual bool IsAnonymous { get { throw null; } }

        public override bool IsAuthenticated { get { throw null; } }

        public virtual bool IsGuest { get { throw null; } }

        public virtual bool IsSystem { get { throw null; } }

        public override string Name { get { throw null; } }

        public SecurityIdentifier Owner { get { throw null; } }

        public SecurityIdentifier User { get { throw null; } }

        public override Claims.ClaimsIdentity Clone() { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public static WindowsIdentity GetAnonymous() { throw null; }

        public static WindowsIdentity GetCurrent() { throw null; }

        public static WindowsIdentity GetCurrent(bool ifImpersonating) { throw null; }

        public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess) { throw null; }

        public static void RunImpersonated(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle safeAccessTokenHandle, Action action) { }

        public static T RunImpersonated<T>(Microsoft.Win32.SafeHandles.SafeAccessTokenHandle safeAccessTokenHandle, Func<T> func) { throw null; }
    }

    public partial class WindowsPrincipal : Claims.ClaimsPrincipal
    {
        public WindowsPrincipal(WindowsIdentity ntIdentity) { }

        public override IIdentity Identity { get { throw null; } }

        public virtual bool IsInRole(int rid) { throw null; }

        public virtual bool IsInRole(SecurityIdentifier sid) { throw null; }

        public virtual bool IsInRole(WindowsBuiltInRole role) { throw null; }

        public override bool IsInRole(string role) { throw null; }
    }
}