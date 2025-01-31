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
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Permissions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides types supporting Code Access Security (CAS).")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Security.Permissions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.IPermission))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.ISecurityEncodable))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Permissions.CodeAccessSecurityAttribute))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Permissions.SecurityAction))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Permissions.SecurityAttribute))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Permissions.SecurityPermissionAttribute))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Permissions.SecurityPermissionFlag))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Policy.Evidence))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Policy.EvidenceBase))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.SecurityElement))]
namespace System
{
    public sealed partial class ApplicationIdentity : Runtime.Serialization.ISerializable
    {
        public ApplicationIdentity(string applicationIdentityFullName) { }

        public string CodeBase { get { throw null; } }

        public string FullName { get { throw null; } }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public override string ToString() { throw null; }
    }
}

namespace System.Configuration
{
    public sealed partial class ConfigurationPermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        public ConfigurationPermission(Security.Permissions.PermissionState state) { }

        public override Security.IPermission Copy() { throw null; }

        public override void FromXml(Security.SecurityElement securityElement) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed partial class ConfigurationPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public ConfigurationPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public override Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Data.Common
{
    public abstract partial class DBDataPermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        protected DBDataPermission() { }

        protected DBDataPermission(DBDataPermission permission) { }

        protected DBDataPermission(DBDataPermissionAttribute permissionAttribute) { }

        protected DBDataPermission(Security.Permissions.PermissionState state, bool allowBlankPassword) { }

        protected DBDataPermission(Security.Permissions.PermissionState state) { }

        public bool AllowBlankPassword { get { throw null; } set { } }

        public virtual void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior) { }

        protected void Clear() { }

        public override Security.IPermission Copy() { throw null; }

        protected virtual DBDataPermission CreateInstance() { throw null; }

        public override void FromXml(Security.SecurityElement securityElement) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract partial class DBDataPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        protected DBDataPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public bool AllowBlankPassword { get { throw null; } set { } }

        public string ConnectionString { get { throw null; } set { } }

        public KeyRestrictionBehavior KeyRestrictionBehavior { get { throw null; } set { } }

        public string KeyRestrictions { get { throw null; } set { } }

        public bool ShouldSerializeConnectionString() { throw null; }

        public bool ShouldSerializeKeyRestrictions() { throw null; }
    }
}

namespace System.Data.Odbc
{
    public sealed partial class OdbcPermission : Common.DBDataPermission
    {
        public OdbcPermission() { }

        public OdbcPermission(Security.Permissions.PermissionState state, bool allowBlankPassword) { }

        public OdbcPermission(Security.Permissions.PermissionState state) { }

        public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior) { }

        public override Security.IPermission Copy() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class OdbcPermissionAttribute : Common.DBDataPermissionAttribute
    {
        public OdbcPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public override Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Data.OleDb
{
    public sealed partial class OleDbPermission : Common.DBDataPermission
    {
        public OleDbPermission() { }

        public OleDbPermission(Security.Permissions.PermissionState state, bool allowBlankPassword) { }

        public OleDbPermission(Security.Permissions.PermissionState state) { }

        [ComponentModel.Browsable(false)]
        public string Provider { get { throw null; } set { } }

        public override Security.IPermission Copy() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class OleDbPermissionAttribute : Common.DBDataPermissionAttribute
    {
        public OleDbPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        [ComponentModel.Browsable(false)]
        public string Provider { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Data.OracleClient
{
    public sealed partial class OraclePermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        public OraclePermission(Security.Permissions.PermissionState state) { }

        public bool AllowBlankPassword { get { throw null; } set { } }

        public void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior) { }

        public override Security.IPermission Copy() { throw null; }

        public override void FromXml(Security.SecurityElement securityElement) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class OraclePermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public OraclePermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public bool AllowBlankPassword { get { throw null; } set { } }

        public string ConnectionString { get { throw null; } set { } }

        public KeyRestrictionBehavior KeyRestrictionBehavior { get { throw null; } set { } }

        public string KeyRestrictions { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }

        public bool ShouldSerializeConnectionString() { throw null; }

        public bool ShouldSerializeKeyRestrictions() { throw null; }
    }
}

namespace System.Data.SqlClient
{
    public sealed partial class SqlClientPermission : Common.DBDataPermission
    {
        public SqlClientPermission() { }

        public SqlClientPermission(Security.Permissions.PermissionState state, bool allowBlankPassword) { }

        public SqlClientPermission(Security.Permissions.PermissionState state) { }

        public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior) { }

        public override Security.IPermission Copy() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class SqlClientPermissionAttribute : Common.DBDataPermissionAttribute
    {
        public SqlClientPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public override Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Diagnostics
{
    public sealed partial class EventLogPermission : Security.Permissions.ResourcePermissionBase
    {
        public EventLogPermission() { }

        public EventLogPermission(EventLogPermissionAccess permissionAccess, string machineName) { }

        public EventLogPermission(EventLogPermissionEntry[] permissionAccessEntries) { }

        public EventLogPermission(Security.Permissions.PermissionState state) { }

        public EventLogPermissionEntryCollection PermissionEntries { get { throw null; } }
    }

    [Flags]
    public enum EventLogPermissionAccess
    {
        None = 0,
        Browse = 2,
        Instrument = 6,
        Audit = 10,
        Write = 16,
        Administer = 48
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
    public partial class EventLogPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public EventLogPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public string MachineName { get { throw null; } set { } }

        public EventLogPermissionAccess PermissionAccess { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }

    public partial class EventLogPermissionEntry
    {
        public EventLogPermissionEntry(EventLogPermissionAccess permissionAccess, string machineName) { }

        public string MachineName { get { throw null; } }

        public EventLogPermissionAccess PermissionAccess { get { throw null; } }
    }

    public partial class EventLogPermissionEntryCollection : Collections.CollectionBase
    {
        internal EventLogPermissionEntryCollection() { }

        public EventLogPermissionEntry this[int index] { get { throw null; } set { } }

        public int Add(EventLogPermissionEntry value) { throw null; }

        public void AddRange(EventLogPermissionEntry[] value) { }

        public void AddRange(EventLogPermissionEntryCollection value) { }

        public bool Contains(EventLogPermissionEntry value) { throw null; }

        public void CopyTo(EventLogPermissionEntry[] array, int index) { }

        public int IndexOf(EventLogPermissionEntry value) { throw null; }

        public void Insert(int index, EventLogPermissionEntry value) { }

        protected override void OnClear() { }

        protected override void OnInsert(int index, object value) { }

        protected override void OnRemove(int index, object value) { }

        protected override void OnSet(int index, object oldValue, object newValue) { }

        public void Remove(EventLogPermissionEntry value) { }
    }

    public sealed partial class PerformanceCounterPermission : Security.Permissions.ResourcePermissionBase
    {
        public PerformanceCounterPermission() { }

        public PerformanceCounterPermission(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName) { }

        public PerformanceCounterPermission(PerformanceCounterPermissionEntry[] permissionAccessEntries) { }

        public PerformanceCounterPermission(Security.Permissions.PermissionState state) { }

        public PerformanceCounterPermissionEntryCollection PermissionEntries { get { throw null; } }
    }

    [Flags]
    public enum PerformanceCounterPermissionAccess
    {
        None = 0,
        Browse = 1,
        Read = 1,
        Write = 2,
        Instrument = 3,
        Administer = 7
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
    public partial class PerformanceCounterPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public PerformanceCounterPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public string CategoryName { get { throw null; } set { } }

        public string MachineName { get { throw null; } set { } }

        public PerformanceCounterPermissionAccess PermissionAccess { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }

    public partial class PerformanceCounterPermissionEntry
    {
        public PerformanceCounterPermissionEntry(PerformanceCounterPermissionAccess permissionAccess, string machineName, string categoryName) { }

        public string CategoryName { get { throw null; } }

        public string MachineName { get { throw null; } }

        public PerformanceCounterPermissionAccess PermissionAccess { get { throw null; } }
    }

    public partial class PerformanceCounterPermissionEntryCollection : Collections.CollectionBase
    {
        internal PerformanceCounterPermissionEntryCollection() { }

        public PerformanceCounterPermissionEntry this[int index] { get { throw null; } set { } }

        public int Add(PerformanceCounterPermissionEntry value) { throw null; }

        public void AddRange(PerformanceCounterPermissionEntry[] value) { }

        public void AddRange(PerformanceCounterPermissionEntryCollection value) { }

        public bool Contains(PerformanceCounterPermissionEntry value) { throw null; }

        public void CopyTo(PerformanceCounterPermissionEntry[] array, int index) { }

        public int IndexOf(PerformanceCounterPermissionEntry value) { throw null; }

        public void Insert(int index, PerformanceCounterPermissionEntry value) { }

        protected override void OnClear() { }

        protected override void OnInsert(int index, object value) { }

        protected override void OnRemove(int index, object value) { }

        protected override void OnSet(int index, object oldValue, object newValue) { }

        public void Remove(PerformanceCounterPermissionEntry value) { }
    }
}

namespace System.Drawing.Printing
{
    public sealed partial class PrintingPermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        public PrintingPermission(PrintingPermissionLevel printingLevel) { }

        public PrintingPermission(Security.Permissions.PermissionState state) { }

        public PrintingPermissionLevel Level { get { throw null; } set { } }

        public override Security.IPermission Copy() { throw null; }

        public override void FromXml(Security.SecurityElement element) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed partial class PrintingPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public PrintingPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public PrintingPermissionLevel Level { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }

    public enum PrintingPermissionLevel
    {
        NoPrinting = 0,
        SafePrinting = 1,
        DefaultPrinting = 2,
        AllPrinting = 3
    }
}

namespace System.Net
{
    public sealed partial class DnsPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public DnsPermission(System.Security.Permissions.PermissionState state) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement securityElement) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class DnsPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public DnsPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }

    public partial class EndpointPermission
    {
        internal EndpointPermission() { }

        public string Hostname { get { throw null; } }

        public int Port { get { throw null; } }

        public TransportType Transport { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    [Flags]
    public enum NetworkAccess
    {
        Connect = 64,
        Accept = 128
    }

    public sealed partial class SocketPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public const int AllPorts = -1;
        public SocketPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber) { }

        public SocketPermission(System.Security.Permissions.PermissionState state) { }

        public Collections.IEnumerator AcceptList { get { throw null; } }

        public Collections.IEnumerator ConnectList { get { throw null; } }

        public void AddPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement securityElement) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class SocketPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public SocketPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public string Access { get { throw null; } set { } }

        public string Host { get { throw null; } set { } }

        public string Port { get { throw null; } set { } }

        public string Transport { get { throw null; } set { } }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }

    public enum TransportType
    {
        Connectionless = 1,
        Udp = 1,
        ConnectionOriented = 2,
        Tcp = 2,
        All = 3
    }

    public sealed partial class WebPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public WebPermission() { }

        public WebPermission(NetworkAccess access, string uriString) { }

        public WebPermission(NetworkAccess access, Text.RegularExpressions.Regex uriRegex) { }

        public WebPermission(System.Security.Permissions.PermissionState state) { }

        public Collections.IEnumerator AcceptList { get { throw null; } }

        public Collections.IEnumerator ConnectList { get { throw null; } }

        public void AddPermission(NetworkAccess access, string uriString) { }

        public void AddPermission(NetworkAccess access, Text.RegularExpressions.Regex uriRegex) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement securityElement) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class WebPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public WebPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public string Accept { get { throw null; } set { } }

        public string AcceptPattern { get { throw null; } set { } }

        public string Connect { get { throw null; } set { } }

        public string ConnectPattern { get { throw null; } set { } }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Net.Mail
{
    public enum SmtpAccess
    {
        None = 0,
        Connect = 1,
        ConnectToUnrestrictedPort = 2
    }

    public sealed partial class SmtpPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public SmtpPermission(bool unrestricted) { }

        public SmtpPermission(SmtpAccess access) { }

        public SmtpPermission(System.Security.Permissions.PermissionState state) { }

        public SmtpAccess Access { get { throw null; } }

        public void AddPermission(SmtpAccess access) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement securityElement) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class SmtpPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public SmtpPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public string Access { get { throw null; } set { } }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Net.NetworkInformation
{
    [Flags]
    public enum NetworkInformationAccess
    {
        None = 0,
        Read = 1,
        Ping = 4
    }

    public sealed partial class NetworkInformationPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public NetworkInformationPermission(NetworkInformationAccess access) { }

        public NetworkInformationPermission(System.Security.Permissions.PermissionState state) { }

        public NetworkInformationAccess Access { get { throw null; } }

        public void AddPermission(NetworkInformationAccess access) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement securityElement) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class NetworkInformationPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public NetworkInformationPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public string Access { get { throw null; } set { } }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Net.PeerToPeer
{
    public sealed partial class PnrpPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public PnrpPermission(System.Security.Permissions.PermissionState state) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement e) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class PnrpPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public PnrpPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }

    public enum PnrpScope
    {
        All = 0,
        Global = 1,
        SiteLocal = 2,
        LinkLocal = 3
    }
}

namespace System.Net.PeerToPeer.Collaboration
{
    public sealed partial class PeerCollaborationPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission
    {
        public PeerCollaborationPermission(System.Security.Permissions.PermissionState state) { }

        public override System.Security.IPermission Copy() { throw null; }

        public override void FromXml(System.Security.SecurityElement e) { }

        public override System.Security.IPermission Intersect(System.Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(System.Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override System.Security.SecurityElement ToXml() { throw null; }

        public override System.Security.IPermission Union(System.Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class PeerCollaborationPermissionAttribute : System.Security.Permissions.CodeAccessSecurityAttribute
    {
        public PeerCollaborationPermissionAttribute(System.Security.Permissions.SecurityAction action) : base(default) { }

        public override System.Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Security
{
    public abstract partial class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
    {
        public void Assert() { }

        public abstract IPermission Copy();
        public void Demand() { }

        [Obsolete]
        public void Deny() { }

        public override bool Equals(object obj) { throw null; }

        public abstract void FromXml(SecurityElement elem);
        public override int GetHashCode() { throw null; }

        public abstract IPermission Intersect(IPermission target);
        public abstract bool IsSubsetOf(IPermission target);
        public void PermitOnly() { }

        public static void RevertAll() { }

        public static void RevertAssert() { }

        [Obsolete]
        public static void RevertDeny() { }

        public static void RevertPermitOnly() { }

        public override string ToString() { throw null; }

        public abstract SecurityElement ToXml();
        public virtual IPermission Union(IPermission other) { throw null; }
    }

    public partial class HostProtectionException : SystemException
    {
        public HostProtectionException() { }

        protected HostProtectionException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public HostProtectionException(string message, Exception e) { }

        public HostProtectionException(string message, Permissions.HostProtectionResource protectedResources, Permissions.HostProtectionResource demandedResources) { }

        public HostProtectionException(string message) { }

        public Permissions.HostProtectionResource DemandedResources { get { throw null; } }

        public Permissions.HostProtectionResource ProtectedResources { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public override string ToString() { throw null; }
    }

    public partial class HostSecurityManager
    {
        public virtual Policy.PolicyLevel DomainPolicy { get { throw null; } }

        public virtual HostSecurityManagerOptions Flags { get { throw null; } }

        public virtual Policy.ApplicationTrust DetermineApplicationTrust(Policy.Evidence applicationEvidence, Policy.Evidence activatorEvidence, Policy.TrustManagerContext context) { throw null; }

        public virtual Policy.EvidenceBase GenerateAppDomainEvidence(Type evidenceType) { throw null; }

        public virtual Policy.EvidenceBase GenerateAssemblyEvidence(Type evidenceType, Reflection.Assembly assembly) { throw null; }

        public virtual Type[] GetHostSuppliedAppDomainEvidenceTypes() { throw null; }

        public virtual Type[] GetHostSuppliedAssemblyEvidenceTypes(Reflection.Assembly assembly) { throw null; }

        public virtual Policy.Evidence ProvideAppDomainEvidence(Policy.Evidence inputEvidence) { throw null; }

        public virtual Policy.Evidence ProvideAssemblyEvidence(Reflection.Assembly loadedAssembly, Policy.Evidence inputEvidence) { throw null; }

        [Obsolete]
        public virtual PermissionSet ResolvePolicy(Policy.Evidence evidence) { throw null; }
    }

    [Flags]
    public enum HostSecurityManagerOptions
    {
        None = 0,
        HostAppDomainEvidence = 1,
        HostPolicyLevel = 2,
        HostAssemblyEvidence = 4,
        HostDetermineApplicationTrust = 8,
        HostResolvePolicy = 16,
        AllFlags = 31
    }

    public partial interface IEvidenceFactory
    {
        Policy.Evidence Evidence { get; }
    }

    public partial interface ISecurityPolicyEncodable
    {
        void FromXml(SecurityElement e, Policy.PolicyLevel level);
        SecurityElement ToXml(Policy.PolicyLevel level);
    }

    public partial interface IStackWalk
    {
        void Assert();
        void Demand();
        void Deny();
        void PermitOnly();
    }

    public sealed partial class NamedPermissionSet : PermissionSet
    {
        public NamedPermissionSet(NamedPermissionSet permSet) : base(default(Permissions.PermissionState)) { }

        public NamedPermissionSet(string name, Permissions.PermissionState state) : base(default(Permissions.PermissionState)) { }

        public NamedPermissionSet(string name, PermissionSet permSet) : base(default(Permissions.PermissionState)) { }

        public NamedPermissionSet(string name) : base(default(Permissions.PermissionState)) { }

        public string Description { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public override PermissionSet Copy() { throw null; }

        public NamedPermissionSet Copy(string name) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override void FromXml(SecurityElement et) { }

        public override int GetHashCode() { throw null; }

        public override SecurityElement ToXml() { throw null; }
    }

    public partial class PermissionSet : Collections.ICollection, Collections.IEnumerable, Runtime.Serialization.IDeserializationCallback, ISecurityEncodable, IStackWalk
    {
        public PermissionSet(Permissions.PermissionState state) { }

        public PermissionSet(PermissionSet? permSet) { }

        public virtual int Count { get { throw null; } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public IPermission? AddPermission(IPermission? perm) { throw null; }

        protected virtual IPermission? AddPermissionImpl(IPermission? perm) { throw null; }

        public void Assert() { }

        public bool ContainsNonCodeAccessPermissions() { throw null; }

        [Obsolete]
        public static byte[] ConvertPermissionSet(string inFormat, byte[] inData, string outFormat) { throw null; }

        public virtual PermissionSet Copy() { throw null; }

        public virtual void CopyTo(Array array, int index) { }

        public void Demand() { }

        [Obsolete]
        public void Deny() { }

        public override bool Equals(object? o) { throw null; }

        public virtual void FromXml(SecurityElement et) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        protected virtual Collections.IEnumerator GetEnumeratorImpl() { throw null; }

        public override int GetHashCode() { throw null; }

        public IPermission? GetPermission(Type? permClass) { throw null; }

        protected virtual IPermission? GetPermissionImpl(Type? permClass) { throw null; }

        public PermissionSet? Intersect(PermissionSet? other) { throw null; }

        public bool IsEmpty() { throw null; }

        public bool IsSubsetOf(PermissionSet? target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public void PermitOnly() { }

        public IPermission? RemovePermission(Type? permClass) { throw null; }

        protected virtual IPermission? RemovePermissionImpl(Type? permClass) { throw null; }

        public static void RevertAssert() { }

        public IPermission? SetPermission(IPermission? perm) { throw null; }

        protected virtual IPermission? SetPermissionImpl(IPermission? perm) { throw null; }

        void Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender) { }

        public override string ToString() { throw null; }

        public virtual SecurityElement? ToXml() { throw null; }

        public PermissionSet? Union(PermissionSet? other) { throw null; }
    }

    public enum PolicyLevelType
    {
        User = 0,
        Machine = 1,
        Enterprise = 2,
        AppDomain = 3
    }

    public sealed partial class SecurityContext : IDisposable
    {
        internal SecurityContext() { }

        public static SecurityContext Capture() { throw null; }

        public SecurityContext CreateCopy() { throw null; }

        public void Dispose() { }

        public static bool IsFlowSuppressed() { throw null; }

        public static bool IsWindowsIdentityFlowSuppressed() { throw null; }

        public static void RestoreFlow() { }

        public static void Run(SecurityContext securityContext, Threading.ContextCallback callback, object state) { }

        public static Threading.AsyncFlowControl SuppressFlow() { throw null; }

        public static Threading.AsyncFlowControl SuppressFlowWindowsIdentity() { throw null; }
    }

    public enum SecurityContextSource
    {
        CurrentAppDomain = 0,
        CurrentAssembly = 1
    }

    public static partial class SecurityManager
    {
        [Obsolete]
        public static bool CheckExecutionRights { get { throw null; } set { } }

        [Obsolete]
        public static bool SecurityEnabled { get { throw null; } set { } }

        public static bool CurrentThreadRequiresSecurityContextCapture() { throw null; }

        public static PermissionSet GetStandardSandbox(Policy.Evidence evidence) { throw null; }

        public static void GetZoneAndOrigin(out Collections.ArrayList zone, out Collections.ArrayList origin) { throw null; }

        [Obsolete]
        public static bool IsGranted(IPermission perm) { throw null; }

        [Obsolete]
        public static Policy.PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type) { throw null; }

        [Obsolete]
        public static Policy.PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type) { throw null; }

        [Obsolete]
        public static Collections.IEnumerator PolicyHierarchy() { throw null; }

        [Obsolete]
        public static PermissionSet ResolvePolicy(Policy.Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied) { throw null; }

        [Obsolete]
        public static PermissionSet ResolvePolicy(Policy.Evidence evidence) { throw null; }

        [Obsolete]
        public static PermissionSet ResolvePolicy(Policy.Evidence[] evidences) { throw null; }

        [Obsolete]
        public static Collections.IEnumerator ResolvePolicyGroups(Policy.Evidence evidence) { throw null; }

        [Obsolete]
        public static PermissionSet ResolveSystemPolicy(Policy.Evidence evidence) { throw null; }

        [Obsolete]
        public static void SavePolicy() { }

        [Obsolete]
        public static void SavePolicyLevel(Policy.PolicyLevel level) { }
    }

    public abstract partial class SecurityState
    {
        public abstract void EnsureState();
        public bool IsStateAvailable() { throw null; }
    }

    public enum SecurityZone
    {
        NoZone = -1,
        MyComputer = 0,
        Intranet = 1,
        Trusted = 2,
        Internet = 3,
        Untrusted = 4
    }

    public sealed partial class XmlSyntaxException : SystemException
    {
        public XmlSyntaxException() { }

        public XmlSyntaxException(int lineNumber, string message) { }

        public XmlSyntaxException(int lineNumber) { }

        public XmlSyntaxException(string message, Exception inner) { }

        public XmlSyntaxException(string message) { }
    }
}

namespace System.Security.Permissions
{
    public sealed partial class DataProtectionPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public DataProtectionPermission(DataProtectionPermissionFlags flag) { }

        public DataProtectionPermission(PermissionState state) { }

        public DataProtectionPermissionFlags Flags { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class DataProtectionPermissionAttribute : CodeAccessSecurityAttribute
    {
        public DataProtectionPermissionAttribute(SecurityAction action) : base(default) { }

        public DataProtectionPermissionFlags Flags { get { throw null; } set { } }

        public bool ProtectData { get { throw null; } set { } }

        public bool ProtectMemory { get { throw null; } set { } }

        public bool UnprotectData { get { throw null; } set { } }

        public bool UnprotectMemory { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    [Flags]
    public enum DataProtectionPermissionFlags
    {
        NoFlags = 0,
        ProtectData = 1,
        UnprotectData = 2,
        ProtectMemory = 4,
        UnprotectMemory = 8,
        AllFlags = 15
    }

    public sealed partial class EnvironmentPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public EnvironmentPermission(EnvironmentPermissionAccess flag, string pathList) { }

        public EnvironmentPermission(PermissionState state) { }

        public void AddPathList(EnvironmentPermissionAccess flag, string pathList) { }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public string GetPathList(EnvironmentPermissionAccess flag) { throw null; }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public void SetPathList(EnvironmentPermissionAccess flag, string pathList) { }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission other) { throw null; }
    }

    [Flags]
    public enum EnvironmentPermissionAccess
    {
        NoAccess = 0,
        Read = 1,
        Write = 2,
        AllAccess = 3
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class EnvironmentPermissionAttribute : CodeAccessSecurityAttribute
    {
        public EnvironmentPermissionAttribute(SecurityAction action) : base(default) { }

        public string All { get { throw null; } set { } }

        public string Read { get { throw null; } set { } }

        public string Write { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class FileDialogPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public FileDialogPermission(FileDialogPermissionAccess access) { }

        public FileDialogPermission(PermissionState state) { }

        public FileDialogPermissionAccess Access { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [Flags]
    public enum FileDialogPermissionAccess
    {
        None = 0,
        Open = 1,
        Save = 2,
        OpenSave = 3
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class FileDialogPermissionAttribute : CodeAccessSecurityAttribute
    {
        public FileDialogPermissionAttribute(SecurityAction action) : base(default) { }

        public bool Open { get { throw null; } set { } }

        public bool Save { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class FileIOPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public FileIOPermission(FileIOPermissionAccess access, AccessControl.AccessControlActions actions, string path) { }

        public FileIOPermission(FileIOPermissionAccess access, AccessControl.AccessControlActions actions, string[] pathList) { }

        public FileIOPermission(FileIOPermissionAccess access, string path) { }

        public FileIOPermission(FileIOPermissionAccess access, string[] pathList) { }

        public FileIOPermission(PermissionState state) { }

        public FileIOPermissionAccess AllFiles { get { throw null; } set { } }

        public FileIOPermissionAccess AllLocalFiles { get { throw null; } set { } }

        public void AddPathList(FileIOPermissionAccess access, string path) { }

        public void AddPathList(FileIOPermissionAccess access, string[] pathList) { }

        public override IPermission Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override int GetHashCode() { throw null; }

        public string[] GetPathList(FileIOPermissionAccess access) { throw null; }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public void SetPathList(FileIOPermissionAccess access, string path) { }

        public void SetPathList(FileIOPermissionAccess access, string[] pathList) { }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission other) { throw null; }
    }

    [Flags]
    public enum FileIOPermissionAccess
    {
        NoAccess = 0,
        Read = 1,
        Write = 2,
        Append = 4,
        PathDiscovery = 8,
        AllAccess = 15
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class FileIOPermissionAttribute : CodeAccessSecurityAttribute
    {
        public FileIOPermissionAttribute(SecurityAction action) : base(default) { }

        [Obsolete]
        public string All { get { throw null; } set { } }

        public FileIOPermissionAccess AllFiles { get { throw null; } set { } }

        public FileIOPermissionAccess AllLocalFiles { get { throw null; } set { } }

        public string Append { get { throw null; } set { } }

        public string ChangeAccessControl { get { throw null; } set { } }

        public string PathDiscovery { get { throw null; } set { } }

        public string Read { get { throw null; } set { } }

        public string ViewAccessControl { get { throw null; } set { } }

        public string ViewAndModify { get { throw null; } set { } }

        public string Write { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class GacIdentityPermission : CodeAccessPermission
    {
        public GacIdentityPermission() { }

        public GacIdentityPermission(PermissionState state) { }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class GacIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public GacIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public override IPermission CreatePermission() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
    public sealed partial class HostProtectionAttribute : CodeAccessSecurityAttribute
    {
        public HostProtectionAttribute() : base(default) { }

        public HostProtectionAttribute(SecurityAction action) : base(default) { }

        public bool ExternalProcessMgmt { get { throw null; } set { } }

        public bool ExternalThreading { get { throw null; } set { } }

        public bool MayLeakOnAbort { get { throw null; } set { } }

        public HostProtectionResource Resources { get { throw null; } set { } }

        public bool SecurityInfrastructure { get { throw null; } set { } }

        public bool SelfAffectingProcessMgmt { get { throw null; } set { } }

        public bool SelfAffectingThreading { get { throw null; } set { } }

        public bool SharedState { get { throw null; } set { } }

        public bool Synchronization { get { throw null; } set { } }

        public bool UI { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    [Flags]
    public enum HostProtectionResource
    {
        None = 0,
        Synchronization = 1,
        SharedState = 2,
        ExternalProcessMgmt = 4,
        SelfAffectingProcessMgmt = 8,
        ExternalThreading = 16,
        SelfAffectingThreading = 32,
        SecurityInfrastructure = 64,
        UI = 128,
        MayLeakOnAbort = 256,
        All = 511
    }

    public enum IsolatedStorageContainment
    {
        None = 0,
        DomainIsolationByUser = 16,
        ApplicationIsolationByUser = 21,
        AssemblyIsolationByUser = 32,
        DomainIsolationByMachine = 48,
        AssemblyIsolationByMachine = 64,
        ApplicationIsolationByMachine = 69,
        DomainIsolationByRoamingUser = 80,
        AssemblyIsolationByRoamingUser = 96,
        ApplicationIsolationByRoamingUser = 101,
        AdministerIsolatedStorageByUser = 112,
        UnrestrictedIsolatedStorage = 240
    }

    public sealed partial class IsolatedStorageFilePermission : IsolatedStoragePermission
    {
        public IsolatedStorageFilePermission(PermissionState state) : base(default) { }

        public override IPermission Copy() { throw null; }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class IsolatedStorageFilePermissionAttribute : IsolatedStoragePermissionAttribute
    {
        public IsolatedStorageFilePermissionAttribute(SecurityAction action) : base(default) { }

        public override IPermission CreatePermission() { throw null; }
    }

    public abstract partial class IsolatedStoragePermission : CodeAccessPermission, IUnrestrictedPermission
    {
        protected IsolatedStoragePermission(PermissionState state) { }

        public IsolatedStorageContainment UsageAllowed { get { throw null; } set { } }

        public long UserQuota { get { throw null; } set { } }

        public override void FromXml(SecurityElement esd) { }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }
    }

    public abstract partial class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
    {
        protected IsolatedStoragePermissionAttribute(SecurityAction action) : base(default) { }

        public IsolatedStorageContainment UsageAllowed { get { throw null; } set { } }

        public long UserQuota { get { throw null; } set { } }
    }

    public partial interface IUnrestrictedPermission
    {
        bool IsUnrestricted();
    }

    public sealed partial class KeyContainerPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public KeyContainerPermission(KeyContainerPermissionFlags flags, KeyContainerPermissionAccessEntry[] accessList) { }

        public KeyContainerPermission(KeyContainerPermissionFlags flags) { }

        public KeyContainerPermission(PermissionState state) { }

        public KeyContainerPermissionAccessEntryCollection AccessEntries { get { throw null; } }

        public KeyContainerPermissionFlags Flags { get { throw null; } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    public sealed partial class KeyContainerPermissionAccessEntry
    {
        public KeyContainerPermissionAccessEntry(Cryptography.CspParameters parameters, KeyContainerPermissionFlags flags) { }

        public KeyContainerPermissionAccessEntry(string keyContainerName, KeyContainerPermissionFlags flags) { }

        public KeyContainerPermissionAccessEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec, KeyContainerPermissionFlags flags) { }

        public KeyContainerPermissionFlags Flags { get { throw null; } set { } }

        public string KeyContainerName { get { throw null; } set { } }

        public int KeySpec { get { throw null; } set { } }

        public string KeyStore { get { throw null; } set { } }

        public string ProviderName { get { throw null; } set { } }

        public int ProviderType { get { throw null; } set { } }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class KeyContainerPermissionAccessEntryCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public KeyContainerPermissionAccessEntry this[int index] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public int Add(KeyContainerPermissionAccessEntry accessEntry) { throw null; }

        public void Clear() { }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index) { }

        public KeyContainerPermissionAccessEntryEnumerator GetEnumerator() { throw null; }

        public int IndexOf(KeyContainerPermissionAccessEntry accessEntry) { throw null; }

        public void Remove(KeyContainerPermissionAccessEntry accessEntry) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class KeyContainerPermissionAccessEntryEnumerator : Collections.IEnumerator
    {
        public KeyContainerPermissionAccessEntry Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class KeyContainerPermissionAttribute : CodeAccessSecurityAttribute
    {
        public KeyContainerPermissionAttribute(SecurityAction action) : base(default) { }

        public KeyContainerPermissionFlags Flags { get { throw null; } set { } }

        public string KeyContainerName { get { throw null; } set { } }

        public int KeySpec { get { throw null; } set { } }

        public string KeyStore { get { throw null; } set { } }

        public string ProviderName { get { throw null; } set { } }

        public int ProviderType { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public enum KeyContainerPermissionFlags
    {
        NoFlags = 0,
        Create = 1,
        Open = 2,
        Delete = 4,
        Import = 16,
        Export = 32,
        Sign = 256,
        Decrypt = 512,
        ViewAcl = 4096,
        ChangeAcl = 8192,
        AllFlags = 13111
    }

    public sealed partial class MediaPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public MediaPermission() { }

        public MediaPermission(MediaPermissionAudio permissionAudio, MediaPermissionVideo permissionVideo, MediaPermissionImage permissionImage) { }

        public MediaPermission(MediaPermissionAudio permissionAudio) { }

        public MediaPermission(MediaPermissionImage permissionImage) { }

        public MediaPermission(MediaPermissionVideo permissionVideo) { }

        public MediaPermission(PermissionState state) { }

        public MediaPermissionAudio Audio { get { throw null; } }

        public MediaPermissionImage Image { get { throw null; } }

        public MediaPermissionVideo Video { get { throw null; } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class MediaPermissionAttribute : CodeAccessSecurityAttribute
    {
        public MediaPermissionAttribute(SecurityAction action) : base(default) { }

        public MediaPermissionAudio Audio { get { throw null; } set { } }

        public MediaPermissionImage Image { get { throw null; } set { } }

        public MediaPermissionVideo Video { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public enum MediaPermissionAudio
    {
        NoAudio = 0,
        SiteOfOriginAudio = 1,
        SafeAudio = 2,
        AllAudio = 3
    }

    public enum MediaPermissionImage
    {
        NoImage = 0,
        SiteOfOriginImage = 1,
        SafeImage = 2,
        AllImage = 3
    }

    public enum MediaPermissionVideo
    {
        NoVideo = 0,
        SiteOfOriginVideo = 1,
        SafeVideo = 2,
        AllVideo = 3
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class PermissionSetAttribute : CodeAccessSecurityAttribute
    {
        public PermissionSetAttribute(SecurityAction action) : base(default) { }

        public string File { get { throw null; } set { } }

        public string Hex { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public bool UnicodeEncoded { get { throw null; } set { } }

        public string XML { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }

        public PermissionSet CreatePermissionSet() { throw null; }
    }

    public enum PermissionState
    {
        None = 0,
        Unrestricted = 1
    }

    public sealed partial class PrincipalPermission : IPermission, ISecurityEncodable, IUnrestrictedPermission
    {
        public PrincipalPermission(PermissionState state) { }

        public PrincipalPermission(string name, string role, bool isAuthenticated) { }

        public PrincipalPermission(string name, string role) { }

        public IPermission Copy() { throw null; }

        public void Demand() { }

        public override bool Equals(object obj) { throw null; }

        public void FromXml(SecurityElement elem) { }

        public override int GetHashCode() { throw null; }

        public IPermission Intersect(IPermission target) { throw null; }

        public bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public IPermission Union(IPermission other) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
    {
        public PrincipalPermissionAttribute(SecurityAction action) : base(default) { }

        public bool Authenticated { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string Role { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class PublisherIdentityPermission : CodeAccessPermission
    {
        public PublisherIdentityPermission(Cryptography.X509Certificates.X509Certificate certificate) { }

        public PublisherIdentityPermission(PermissionState state) { }

        public Cryptography.X509Certificates.X509Certificate Certificate { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public PublisherIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public string CertFile { get { throw null; } set { } }

        public string SignedFile { get { throw null; } set { } }

        public string X509Certificate { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class ReflectionPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public ReflectionPermission(PermissionState state) { }

        public ReflectionPermission(ReflectionPermissionFlag flag) { }

        public ReflectionPermissionFlag Flags { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission other) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class ReflectionPermissionAttribute : CodeAccessSecurityAttribute
    {
        public ReflectionPermissionAttribute(SecurityAction action) : base(default) { }

        public ReflectionPermissionFlag Flags { get { throw null; } set { } }

        public bool MemberAccess { get { throw null; } set { } }

        [Obsolete("ReflectionPermissionAttribute.ReflectionEmit has been deprecated and is not supported.")]
        public bool ReflectionEmit { get { throw null; } set { } }

        public bool RestrictedMemberAccess { get { throw null; } set { } }

        [Obsolete("ReflectionPermissionAttribute.TypeInformation has been deprecated and is not supported.")]
        public bool TypeInformation { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    [Flags]
    public enum ReflectionPermissionFlag
    {
        NoFlags = 0,
        TypeInformation = 1,
        MemberAccess = 2,
        ReflectionEmit = 4,
        AllFlags = 7,
        RestrictedMemberAccess = 8
    }

    public sealed partial class RegistryPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public RegistryPermission(PermissionState state) { }

        public RegistryPermission(RegistryPermissionAccess access, AccessControl.AccessControlActions control, string pathList) { }

        public RegistryPermission(RegistryPermissionAccess access, string pathList) { }

        public void AddPathList(RegistryPermissionAccess access, AccessControl.AccessControlActions actions, string pathList) { }

        public void AddPathList(RegistryPermissionAccess access, string pathList) { }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement elem) { }

        public string GetPathList(RegistryPermissionAccess access) { throw null; }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public void SetPathList(RegistryPermissionAccess access, string pathList) { }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission other) { throw null; }
    }

    [Flags]
    public enum RegistryPermissionAccess
    {
        NoAccess = 0,
        Read = 1,
        Write = 2,
        Create = 4,
        AllAccess = 7
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class RegistryPermissionAttribute : CodeAccessSecurityAttribute
    {
        public RegistryPermissionAttribute(SecurityAction action) : base(default) { }

        [Obsolete("RegistryPermissionAttribute.Add has been deprecated. Use ViewAndModify instead.")]
        public string All { get { throw null; } set { } }

        public string ChangeAccessControl { get { throw null; } set { } }

        public string Create { get { throw null; } set { } }

        public string Read { get { throw null; } set { } }

        public string ViewAccessControl { get { throw null; } set { } }

        public string ViewAndModify { get { throw null; } set { } }

        public string Write { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public abstract partial class ResourcePermissionBase : CodeAccessPermission, IUnrestrictedPermission
    {
        public const string Any = "*";
        public const string Local = ".";
        protected ResourcePermissionBase() { }

        protected ResourcePermissionBase(PermissionState state) { }

        protected Type PermissionAccessType { get { throw null; } set { } }

        protected string[] TagNames { get { throw null; } set { } }

        protected void AddPermissionAccess(ResourcePermissionBaseEntry entry) { }

        protected void Clear() { }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        protected ResourcePermissionBaseEntry[] GetPermissionEntries() { throw null; }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        protected void RemovePermissionAccess(ResourcePermissionBaseEntry entry) { }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    public partial class ResourcePermissionBaseEntry
    {
        public ResourcePermissionBaseEntry() { }

        public ResourcePermissionBaseEntry(int permissionAccess, string[] permissionAccessPath) { }

        public int PermissionAccess { get { throw null; } }

        public string[] PermissionAccessPath { get { throw null; } }
    }

    public sealed partial class SecurityPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public SecurityPermission(PermissionState state) { }

        public SecurityPermission(SecurityPermissionFlag flag) { }

        public SecurityPermissionFlag Flags { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    public sealed partial class SiteIdentityPermission : CodeAccessPermission
    {
        public SiteIdentityPermission(PermissionState state) { }

        public SiteIdentityPermission(string site) { }

        public string Site { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class SiteIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public SiteIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public string Site { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class StorePermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public StorePermission(PermissionState state) { }

        public StorePermission(StorePermissionFlags flag) { }

        public StorePermissionFlags Flags { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class StorePermissionAttribute : CodeAccessSecurityAttribute
    {
        public StorePermissionAttribute(SecurityAction action) : base(default) { }

        public bool AddToStore { get { throw null; } set { } }

        public bool CreateStore { get { throw null; } set { } }

        public bool DeleteStore { get { throw null; } set { } }

        public bool EnumerateCertificates { get { throw null; } set { } }

        public bool EnumerateStores { get { throw null; } set { } }

        public StorePermissionFlags Flags { get { throw null; } set { } }

        public bool OpenStore { get { throw null; } set { } }

        public bool RemoveFromStore { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    [Flags]
    public enum StorePermissionFlags
    {
        NoFlags = 0,
        CreateStore = 1,
        DeleteStore = 2,
        EnumerateStores = 4,
        OpenStore = 16,
        AddToStore = 32,
        RemoveFromStore = 64,
        EnumerateCertificates = 128,
        AllFlags = 247
    }

    public sealed partial class StrongNameIdentityPermission : CodeAccessPermission
    {
        public StrongNameIdentityPermission(PermissionState state) { }

        public StrongNameIdentityPermission(StrongNamePublicKeyBlob blob, string name, Version version) { }

        public string Name { get { throw null; } set { } }

        public StrongNamePublicKeyBlob PublicKey { get { throw null; } set { } }

        public Version Version { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement e) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public StrongNameIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public string Name { get { throw null; } set { } }

        public string PublicKey { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class StrongNamePublicKeyBlob
    {
        public StrongNamePublicKeyBlob(byte[] publicKey) { }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class TypeDescriptorPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public TypeDescriptorPermission(PermissionState state) { }

        public TypeDescriptorPermission(TypeDescriptorPermissionFlags flag) { }

        public TypeDescriptorPermissionFlags Flags { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class TypeDescriptorPermissionAttribute : CodeAccessSecurityAttribute
    {
        public TypeDescriptorPermissionAttribute(SecurityAction action) : base(default) { }

        public TypeDescriptorPermissionFlags Flags { get { throw null; } set { } }

        public bool RestrictedRegistrationAccess { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    [Flags]
    public enum TypeDescriptorPermissionFlags
    {
        NoFlags = 0,
        RestrictedRegistrationAccess = 1
    }

    public sealed partial class UIPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public UIPermission(PermissionState state) { }

        public UIPermission(UIPermissionClipboard clipboardFlag) { }

        public UIPermission(UIPermissionWindow windowFlag, UIPermissionClipboard clipboardFlag) { }

        public UIPermission(UIPermissionWindow windowFlag) { }

        public UIPermissionClipboard Clipboard { get { throw null; } set { } }

        public UIPermissionWindow Window { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class UIPermissionAttribute : CodeAccessSecurityAttribute
    {
        public UIPermissionAttribute(SecurityAction action) : base(default) { }

        public UIPermissionClipboard Clipboard { get { throw null; } set { } }

        public UIPermissionWindow Window { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public enum UIPermissionClipboard
    {
        NoClipboard = 0,
        OwnClipboard = 1,
        AllClipboard = 2
    }

    public enum UIPermissionWindow
    {
        NoWindows = 0,
        SafeSubWindows = 1,
        SafeTopLevelWindows = 2,
        AllWindows = 3
    }

    public sealed partial class UrlIdentityPermission : CodeAccessPermission
    {
        public UrlIdentityPermission(PermissionState state) { }

        public UrlIdentityPermission(string site) { }

        public string Url { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class UrlIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public UrlIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public string Url { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public sealed partial class WebBrowserPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        public WebBrowserPermission() { }

        public WebBrowserPermission(PermissionState state) { }

        public WebBrowserPermission(WebBrowserPermissionLevel webBrowserPermissionLevel) { }

        public WebBrowserPermissionLevel Level { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement securityElement) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class WebBrowserPermissionAttribute : CodeAccessSecurityAttribute
    {
        public WebBrowserPermissionAttribute(SecurityAction action) : base(default) { }

        public WebBrowserPermissionLevel Level { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }

    public enum WebBrowserPermissionLevel
    {
        None = 0,
        Safe = 1,
        Unrestricted = 2
    }

    public sealed partial class ZoneIdentityPermission : CodeAccessPermission
    {
        public ZoneIdentityPermission(PermissionState state) { }

        public ZoneIdentityPermission(SecurityZone zone) { }

        public SecurityZone SecurityZone { get { throw null; } set { } }

        public override IPermission Copy() { throw null; }

        public override void FromXml(SecurityElement esd) { }

        public override IPermission Intersect(IPermission target) { throw null; }

        public override bool IsSubsetOf(IPermission target) { throw null; }

        public override SecurityElement ToXml() { throw null; }

        public override IPermission Union(IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed partial class ZoneIdentityPermissionAttribute : CodeAccessSecurityAttribute
    {
        public ZoneIdentityPermissionAttribute(SecurityAction action) : base(default) { }

        public SecurityZone Zone { get { throw null; } set { } }

        public override IPermission CreatePermission() { throw null; }
    }
}

namespace System.Security.Policy
{
    public sealed partial class AllMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class ApplicationDirectory : EvidenceBase
    {
        public ApplicationDirectory(string name) { }

        public string Directory { get { throw null; } }

        public object Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class ApplicationDirectoryMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class ApplicationTrust : EvidenceBase, ISecurityEncodable
    {
        public ApplicationTrust() { }

        public ApplicationTrust(ApplicationIdentity identity) { }

        public ApplicationTrust(PermissionSet defaultGrantSet, Collections.Generic.IEnumerable<StrongName> fullTrustAssemblies) { }

        public ApplicationIdentity ApplicationIdentity { get { throw null; } set { } }

        public PolicyStatement DefaultGrantSet { get { throw null; } set { } }

        public object ExtraInfo { get { throw null; } set { } }

        public Collections.Generic.IList<StrongName> FullTrustAssemblies { get { throw null; } }

        public bool IsApplicationTrustedToRun { get { throw null; } set { } }

        public bool Persist { get { throw null; } set { } }

        public void FromXml(SecurityElement element) { }

        public SecurityElement ToXml() { throw null; }
    }

    public sealed partial class ApplicationTrustCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal ApplicationTrustCollection() { }

        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public ApplicationTrust this[int index] { get { throw null; } }

        public ApplicationTrust this[string appFullName] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public int Add(ApplicationTrust trust) { throw null; }

        public void AddRange(ApplicationTrust[] trusts) { }

        public void AddRange(ApplicationTrustCollection trusts) { }

        public void Clear() { }

        public void CopyTo(ApplicationTrust[] array, int index) { }

        public ApplicationTrustCollection Find(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch) { throw null; }

        public ApplicationTrustEnumerator GetEnumerator() { throw null; }

        public void Remove(ApplicationIdentity applicationIdentity, ApplicationVersionMatch versionMatch) { }

        public void Remove(ApplicationTrust trust) { }

        public void RemoveRange(ApplicationTrust[] trusts) { }

        public void RemoveRange(ApplicationTrustCollection trusts) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class ApplicationTrustEnumerator : Collections.IEnumerator
    {
        internal ApplicationTrustEnumerator() { }

        public ApplicationTrust Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public enum ApplicationVersionMatch
    {
        MatchExactVersion = 0,
        MatchAllVersions = 1
    }

    public partial class CodeConnectAccess
    {
        public static readonly string AnyScheme;
        public static readonly int DefaultPort;
        public static readonly int OriginPort;
        public static readonly string OriginScheme;
        public CodeConnectAccess(string allowScheme, int allowPort) { }

        public int Port { get { throw null; } }

        public string Scheme { get { throw null; } }

        public static CodeConnectAccess CreateAnySchemeAccess(int allowPort) { throw null; }

        public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class CodeGroup
    {
        protected CodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) { }

        public virtual string AttributeString { get { throw null; } }

        public Collections.IList Children { get { throw null; } set { } }

        public string Description { get { throw null; } set { } }

        public IMembershipCondition MembershipCondition { get { throw null; } set { } }

        public abstract string MergeLogic { get; }

        public string Name { get { throw null; } set { } }

        public virtual string PermissionSetName { get { throw null; } }

        public PolicyStatement PolicyStatement { get { throw null; } set { } }

        public void AddChild(CodeGroup group) { }

        public abstract CodeGroup Copy();
        protected virtual void CreateXml(SecurityElement element, PolicyLevel level) { }

        public override bool Equals(object o) { throw null; }

        public bool Equals(CodeGroup cg, bool compareChildren) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        protected virtual void ParseXml(SecurityElement e, PolicyLevel level) { }

        public void RemoveChild(CodeGroup group) { }

        public abstract PolicyStatement Resolve(Evidence evidence);
        public abstract CodeGroup ResolveMatchingCodeGroups(Evidence evidence);
        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class FileCodeGroup : CodeGroup
    {
        public FileCodeGroup(IMembershipCondition membershipCondition, Permissions.FileIOPermissionAccess access) : base(default!, default!) { }

        public override string AttributeString { get { throw null; } }

        public override string MergeLogic { get { throw null; } }

        public override string PermissionSetName { get { throw null; } }

        public override CodeGroup Copy() { throw null; }

        protected override void CreateXml(SecurityElement element, PolicyLevel level) { }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        protected override void ParseXml(SecurityElement e, PolicyLevel level) { }

        public override PolicyStatement Resolve(Evidence evidence) { throw null; }

        public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence) { throw null; }
    }

    [Obsolete("Code Access Security is not supported or honored by the runtime.")]
    public sealed partial class FirstMatchCodeGroup : CodeGroup
    {
        public FirstMatchCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) : base(default!, default!) { }

        public override string MergeLogic { get { throw null; } }

        public override CodeGroup Copy() { throw null; }

        public override PolicyStatement Resolve(Evidence evidence) { throw null; }

        public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence) { throw null; }
    }

    public sealed partial class GacInstalled : EvidenceBase, IIdentityPermissionFactory
    {
        public object Copy() { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class GacMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class Hash : EvidenceBase, Runtime.Serialization.ISerializable
    {
        public Hash(Reflection.Assembly assembly) { }

        public byte[] MD5 { get { throw null; } }

        public byte[] SHA1 { get { throw null; } }

        public byte[] SHA256 { get { throw null; } }

        public static Hash CreateMD5(byte[] md5) { throw null; }

        public static Hash CreateSHA1(byte[] sha1) { throw null; }

        public static Hash CreateSHA256(byte[] sha256) { throw null; }

        public byte[] GenerateHash(Cryptography.HashAlgorithm hashAlg) { throw null; }

        public void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public override string ToString() { throw null; }
    }

    public sealed partial class HashMembershipCondition : Runtime.Serialization.IDeserializationCallback, Runtime.Serialization.ISerializable, ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public HashMembershipCondition(Cryptography.HashAlgorithm hashAlg, byte[] value) { }

        public Cryptography.HashAlgorithm HashAlgorithm { get { throw null; } set { } }

        public byte[] HashValue { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        void Runtime.Serialization.IDeserializationCallback.OnDeserialization(object sender) { }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public partial interface IIdentityPermissionFactory
    {
        IPermission CreateIdentityPermission(Evidence evidence);
    }

    public partial interface IMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable
    {
        bool Check(Evidence evidence);
        IMembershipCondition Copy();
        bool Equals(object obj);
        string ToString();
    }

    public sealed partial class NetCodeGroup : CodeGroup
    {
        public static readonly string AbsentOriginScheme;
        public static readonly string AnyOtherOriginScheme;
        public NetCodeGroup(IMembershipCondition membershipCondition) : base(default!, default!) { }

        public override string AttributeString { get { throw null; } }

        public override string MergeLogic { get { throw null; } }

        public override string PermissionSetName { get { throw null; } }

        public void AddConnectAccess(string originScheme, CodeConnectAccess connectAccess) { }

        public override CodeGroup Copy() { throw null; }

        protected override void CreateXml(SecurityElement element, PolicyLevel level) { }

        public override bool Equals(object o) { throw null; }

        public Collections.DictionaryEntry[] GetConnectAccessRules() { throw null; }

        public override int GetHashCode() { throw null; }

        protected override void ParseXml(SecurityElement e, PolicyLevel level) { }

        public void ResetConnectAccess() { }

        public override PolicyStatement Resolve(Evidence evidence) { throw null; }

        public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence) { throw null; }
    }

    [Obsolete("Code Access Security is not supported or honored by the runtime.")]
    public sealed partial class PermissionRequestEvidence : EvidenceBase
    {
        public PermissionRequestEvidence(PermissionSet request, PermissionSet optional, PermissionSet denied) { }

        public PermissionSet DeniedPermissions { get { throw null; } }

        public PermissionSet OptionalPermissions { get { throw null; } }

        public PermissionSet RequestedPermissions { get { throw null; } }

        public PermissionRequestEvidence Copy() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class PolicyException : SystemException
    {
        public PolicyException() { }

        protected PolicyException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public PolicyException(string message, Exception exception) { }

        public PolicyException(string message) { }
    }

    public sealed partial class PolicyLevel
    {
        internal PolicyLevel() { }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public Collections.IList FullTrustAssemblies { get { throw null; } }

        public string Label { get { throw null; } }

        public Collections.IList NamedPermissionSets { get { throw null; } }

        public CodeGroup RootCodeGroup { get { throw null; } set { } }

        public string StoreLocation { get { throw null; } }

        public PolicyLevelType Type { get { throw null; } }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void AddFullTrustAssembly(StrongName sn) { }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void AddFullTrustAssembly(StrongNameMembershipCondition snMC) { }

        public void AddNamedPermissionSet(NamedPermissionSet permSet) { }

        public NamedPermissionSet ChangeNamedPermissionSet(string name, PermissionSet pSet) { throw null; }

        [Obsolete("Code Access Security is not supported or honored by the runtime.")]
        public static PolicyLevel CreateAppDomainLevel() { throw null; }

        public void FromXml(SecurityElement e) { }

        public NamedPermissionSet GetNamedPermissionSet(string name) { throw null; }

        public void Recover() { }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void RemoveFullTrustAssembly(StrongName sn) { }

        [Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
        public void RemoveFullTrustAssembly(StrongNameMembershipCondition snMC) { }

        public NamedPermissionSet RemoveNamedPermissionSet(NamedPermissionSet permSet) { throw null; }

        public NamedPermissionSet RemoveNamedPermissionSet(string name) { throw null; }

        public void Reset() { }

        public PolicyStatement Resolve(Evidence evidence) { throw null; }

        public CodeGroup ResolveMatchingCodeGroups(Evidence evidence) { throw null; }

        public SecurityElement ToXml() { throw null; }
    }

    public sealed partial class PolicyStatement : ISecurityEncodable, ISecurityPolicyEncodable
    {
        public PolicyStatement(PermissionSet permSet, PolicyStatementAttribute attributes) { }

        public PolicyStatement(PermissionSet permSet) { }

        public PolicyStatementAttribute Attributes { get { throw null; } set { } }

        public string AttributeString { get { throw null; } }

        public PermissionSet PermissionSet { get { throw null; } set { } }

        public PolicyStatement Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement et, PolicyLevel level) { }

        public void FromXml(SecurityElement et) { }

        public override int GetHashCode() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    [Flags]
    public enum PolicyStatementAttribute
    {
        Nothing = 0,
        Exclusive = 1,
        LevelFinal = 2,
        All = 3
    }

    public sealed partial class Publisher : EvidenceBase, IIdentityPermissionFactory
    {
        public Publisher(Cryptography.X509Certificates.X509Certificate cert) { }

        public Cryptography.X509Certificates.X509Certificate Certificate { get { throw null; } }

        public object Copy() { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class PublisherMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public PublisherMembershipCondition(Cryptography.X509Certificates.X509Certificate certificate) { }

        public Cryptography.X509Certificates.X509Certificate Certificate { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class Site : EvidenceBase, IIdentityPermissionFactory
    {
        public Site(string name) { }

        public string Name { get { throw null; } }

        public object Copy() { throw null; }

        public static Site CreateFromUrl(string url) { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class SiteMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public SiteMembershipCondition(string site) { }

        public string Site { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class StrongName : EvidenceBase, IIdentityPermissionFactory
    {
        public StrongName(Permissions.StrongNamePublicKeyBlob blob, string name, Version version) { }

        public string Name { get { throw null; } }

        public Permissions.StrongNamePublicKeyBlob PublicKey { get { throw null; } }

        public Version Version { get { throw null; } }

        public object Copy() { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class StrongNameMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
    {
        public StrongNameMembershipCondition(Permissions.StrongNamePublicKeyBlob blob, string name, Version version) { }

        public string Name { get { throw null; } set { } }

        public Permissions.StrongNamePublicKeyBlob PublicKey { get { throw null; } set { } }

        public Version Version { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public partial class TrustManagerContext
    {
        public TrustManagerContext() { }

        public TrustManagerContext(TrustManagerUIContext uiContext) { }

        public virtual bool IgnorePersistedDecision { get { throw null; } set { } }

        public virtual bool KeepAlive { get { throw null; } set { } }

        public virtual bool NoPrompt { get { throw null; } set { } }

        public virtual bool Persist { get { throw null; } set { } }

        public virtual ApplicationIdentity PreviousApplicationIdentity { get { throw null; } set { } }

        public virtual TrustManagerUIContext UIContext { get { throw null; } set { } }
    }

    public enum TrustManagerUIContext
    {
        Install = 0,
        Upgrade = 1,
        Run = 2
    }

    [Obsolete("Code Access Security is not supported or honored by the runtime.")]
    public sealed partial class UnionCodeGroup : CodeGroup
    {
        public UnionCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) : base(default!, default!) { }

        public override string MergeLogic { get { throw null; } }

        public override CodeGroup Copy() { throw null; }

        public override PolicyStatement Resolve(Evidence evidence) { throw null; }

        public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence) { throw null; }
    }

    public sealed partial class Url : EvidenceBase, IIdentityPermissionFactory
    {
        public Url(string name) { }

        public string Value { get { throw null; } }

        public object Copy() { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class UrlMembershipCondition : ISecurityEncodable, IMembershipCondition, ISecurityPolicyEncodable
    {
        public UrlMembershipCondition(string url) { }

        public string Url { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object obj) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }

    public sealed partial class Zone : EvidenceBase, IIdentityPermissionFactory
    {
        public Zone(SecurityZone zone) { }

        public SecurityZone SecurityZone { get { throw null; } }

        public object Copy() { throw null; }

        public static Zone CreateFromUrl(string url) { throw null; }

        public IPermission CreateIdentityPermission(Evidence evidence) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class ZoneMembershipCondition : ISecurityEncodable, IMembershipCondition, ISecurityPolicyEncodable
    {
        public ZoneMembershipCondition(SecurityZone zone) { }

        public SecurityZone SecurityZone { get { throw null; } set { } }

        public bool Check(Evidence evidence) { throw null; }

        public IMembershipCondition Copy() { throw null; }

        public override bool Equals(object o) { throw null; }

        public void FromXml(SecurityElement e, PolicyLevel level) { }

        public void FromXml(SecurityElement e) { }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public SecurityElement ToXml() { throw null; }

        public SecurityElement ToXml(PolicyLevel level) { throw null; }
    }
}

namespace System.ServiceProcess
{
    public sealed partial class ServiceControllerPermission : Security.Permissions.ResourcePermissionBase
    {
        public ServiceControllerPermission() { }

        public ServiceControllerPermission(Security.Permissions.PermissionState state) { }

        public ServiceControllerPermission(ServiceControllerPermissionAccess permissionAccess, string machineName, string serviceName) { }

        public ServiceControllerPermission(ServiceControllerPermissionEntry[] permissionAccessEntries) { }

        public ServiceControllerPermissionEntryCollection PermissionEntries { get { throw null; } }
    }

    [Flags]
    public enum ServiceControllerPermissionAccess
    {
        None = 0,
        Browse = 2,
        Control = 6
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
    public partial class ServiceControllerPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public ServiceControllerPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public string MachineName { get { throw null; } set { } }

        public ServiceControllerPermissionAccess PermissionAccess { get { throw null; } set { } }

        public string ServiceName { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }

    public partial class ServiceControllerPermissionEntry
    {
        public ServiceControllerPermissionEntry() { }

        public ServiceControllerPermissionEntry(ServiceControllerPermissionAccess permissionAccess, string machineName, string serviceName) { }

        public string MachineName { get { throw null; } }

        public ServiceControllerPermissionAccess PermissionAccess { get { throw null; } }

        public string ServiceName { get { throw null; } }
    }

    public partial class ServiceControllerPermissionEntryCollection : Collections.CollectionBase
    {
        internal ServiceControllerPermissionEntryCollection() { }

        public ServiceControllerPermissionEntry this[int index] { get { throw null; } set { } }

        public int Add(ServiceControllerPermissionEntry value) { throw null; }

        public void AddRange(ServiceControllerPermissionEntry[] value) { }

        public void AddRange(ServiceControllerPermissionEntryCollection value) { }

        public bool Contains(ServiceControllerPermissionEntry value) { throw null; }

        public void CopyTo(ServiceControllerPermissionEntry[] array, int index) { }

        public int IndexOf(ServiceControllerPermissionEntry value) { throw null; }

        public void Insert(int index, ServiceControllerPermissionEntry value) { }

        protected override void OnClear() { }

        protected override void OnInsert(int index, object value) { }

        protected override void OnRemove(int index, object value) { }

        protected override void OnSet(int index, object oldValue, object newValue) { }

        public void Remove(ServiceControllerPermissionEntry value) { }
    }
}

namespace System.Transactions
{
    public sealed partial class DistributedTransactionPermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        public DistributedTransactionPermission(Security.Permissions.PermissionState state) { }

        public override Security.IPermission Copy() { throw null; }

        public override void FromXml(Security.SecurityElement securityElement) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed partial class DistributedTransactionPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public DistributedTransactionPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public bool Unrestricted { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }
}

namespace System.Web
{
    public sealed partial class AspNetHostingPermission : Security.CodeAccessPermission, Security.Permissions.IUnrestrictedPermission
    {
        public AspNetHostingPermission(Security.Permissions.PermissionState state) { }

        public AspNetHostingPermission(AspNetHostingPermissionLevel level) { }

        public AspNetHostingPermissionLevel Level { get { throw null; } set { } }

        public override Security.IPermission Copy() { throw null; }

        public override void FromXml(Security.SecurityElement securityElement) { }

        public override Security.IPermission Intersect(Security.IPermission target) { throw null; }

        public override bool IsSubsetOf(Security.IPermission target) { throw null; }

        public bool IsUnrestricted() { throw null; }

        public override Security.SecurityElement ToXml() { throw null; }

        public override Security.IPermission Union(Security.IPermission target) { throw null; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed partial class AspNetHostingPermissionAttribute : Security.Permissions.CodeAccessSecurityAttribute
    {
        public AspNetHostingPermissionAttribute(Security.Permissions.SecurityAction action) : base(default) { }

        public AspNetHostingPermissionLevel Level { get { throw null; } set { } }

        public override Security.IPermission CreatePermission() { throw null; }
    }

    public enum AspNetHostingPermissionLevel
    {
        None = 100,
        Minimal = 200,
        Low = 300,
        Medium = 400,
        High = 500,
        Unrestricted = 600
    }
}