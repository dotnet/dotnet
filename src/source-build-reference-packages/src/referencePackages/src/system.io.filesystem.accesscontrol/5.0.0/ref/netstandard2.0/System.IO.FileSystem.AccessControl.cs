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
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.FileSystem.AccessControl")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.IO.FileSystem.AccessControl")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.IO.FileSystem.AccessControl")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO
{
    public static partial class FileSystemAclExtensions
    {
        public static void Create(this DirectoryInfo directoryInfo, Security.AccessControl.DirectorySecurity directorySecurity) { }

        public static FileStream Create(this FileInfo fileInfo, FileMode mode, Security.AccessControl.FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, Security.AccessControl.FileSecurity fileSecurity) { throw null; }

        public static DirectoryInfo CreateDirectory(this Security.AccessControl.DirectorySecurity directorySecurity, string path) { throw null; }

        public static Security.AccessControl.DirectorySecurity GetAccessControl(this DirectoryInfo directoryInfo, Security.AccessControl.AccessControlSections includeSections) { throw null; }

        public static Security.AccessControl.DirectorySecurity GetAccessControl(this DirectoryInfo directoryInfo) { throw null; }

        public static Security.AccessControl.FileSecurity GetAccessControl(this FileInfo fileInfo, Security.AccessControl.AccessControlSections includeSections) { throw null; }

        public static Security.AccessControl.FileSecurity GetAccessControl(this FileInfo fileInfo) { throw null; }

        public static Security.AccessControl.FileSecurity GetAccessControl(this FileStream fileStream) { throw null; }

        public static void SetAccessControl(this DirectoryInfo directoryInfo, Security.AccessControl.DirectorySecurity directorySecurity) { }

        public static void SetAccessControl(this FileInfo fileInfo, Security.AccessControl.FileSecurity fileSecurity) { }

        public static void SetAccessControl(this FileStream fileStream, Security.AccessControl.FileSecurity fileSecurity) { }
    }
}

namespace System.Security.AccessControl
{
    public abstract partial class DirectoryObjectSecurity : ObjectSecurity
    {
        protected DirectoryObjectSecurity() { }

        protected DirectoryObjectSecurity(CommonSecurityDescriptor securityDescriptor) { }

        public virtual AccessRule AccessRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectType, Guid inheritedObjectType) { throw null; }

        protected void AddAccessRule(ObjectAccessRule rule) { }

        protected void AddAuditRule(ObjectAuditRule rule) { }

        public virtual AuditRule AuditRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectType, Guid inheritedObjectType) { throw null; }

        public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType) { throw null; }

        public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType) { throw null; }

        protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified) { throw null; }

        protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified) { throw null; }

        protected bool RemoveAccessRule(ObjectAccessRule rule) { throw null; }

        protected void RemoveAccessRuleAll(ObjectAccessRule rule) { }

        protected void RemoveAccessRuleSpecific(ObjectAccessRule rule) { }

        protected bool RemoveAuditRule(ObjectAuditRule rule) { throw null; }

        protected void RemoveAuditRuleAll(ObjectAuditRule rule) { }

        protected void RemoveAuditRuleSpecific(ObjectAuditRule rule) { }

        protected void ResetAccessRule(ObjectAccessRule rule) { }

        protected void SetAccessRule(ObjectAccessRule rule) { }

        protected void SetAuditRule(ObjectAuditRule rule) { }
    }

    public sealed partial class DirectorySecurity : FileSystemSecurity
    {
        public DirectorySecurity() { }

        public DirectorySecurity(string name, AccessControlSections includeSections) { }
    }

    public sealed partial class FileSecurity : FileSystemSecurity
    {
        public FileSecurity() { }

        public FileSecurity(string fileName, AccessControlSections includeSections) { }
    }

    public sealed partial class FileSystemAccessRule : AccessRule
    {
        public FileSystemAccessRule(Principal.IdentityReference identity, FileSystemRights fileSystemRights, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public FileSystemAccessRule(Principal.IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public FileSystemRights FileSystemRights { get { throw null; } }
    }

    public sealed partial class FileSystemAuditRule : AuditRule
    {
        public FileSystemAuditRule(Principal.IdentityReference identity, FileSystemRights fileSystemRights, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public FileSystemAuditRule(Principal.IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public FileSystemRights FileSystemRights { get { throw null; } }
    }

    [Flags]
    public enum FileSystemRights
    {
        ListDirectory = 1,
        ReadData = 1,
        CreateFiles = 2,
        WriteData = 2,
        AppendData = 4,
        CreateDirectories = 4,
        ReadExtendedAttributes = 8,
        WriteExtendedAttributes = 16,
        ExecuteFile = 32,
        Traverse = 32,
        DeleteSubdirectoriesAndFiles = 64,
        ReadAttributes = 128,
        WriteAttributes = 256,
        Write = 278,
        Delete = 65536,
        ReadPermissions = 131072,
        Read = 131209,
        ReadAndExecute = 131241,
        Modify = 197055,
        ChangePermissions = 262144,
        TakeOwnership = 524288,
        Synchronize = 1048576,
        FullControl = 2032127
    }

    public abstract partial class FileSystemSecurity : NativeObjectSecurity
    {
        internal FileSystemSecurity() : base(default, default) { }

        public override Type AccessRightType { get { throw null; } }

        public override Type AccessRuleType { get { throw null; } }

        public override Type AuditRuleType { get { throw null; } }

        public sealed override AccessRule AccessRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) { throw null; }

        public void AddAccessRule(FileSystemAccessRule rule) { }

        public void AddAuditRule(FileSystemAuditRule rule) { }

        public sealed override AuditRule AuditRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) { throw null; }

        public bool RemoveAccessRule(FileSystemAccessRule rule) { throw null; }

        public void RemoveAccessRuleAll(FileSystemAccessRule rule) { }

        public void RemoveAccessRuleSpecific(FileSystemAccessRule rule) { }

        public bool RemoveAuditRule(FileSystemAuditRule rule) { throw null; }

        public void RemoveAuditRuleAll(FileSystemAuditRule rule) { }

        public void RemoveAuditRuleSpecific(FileSystemAuditRule rule) { }

        public void ResetAccessRule(FileSystemAccessRule rule) { }

        public void SetAccessRule(FileSystemAccessRule rule) { }

        public void SetAuditRule(FileSystemAuditRule rule) { }
    }
}