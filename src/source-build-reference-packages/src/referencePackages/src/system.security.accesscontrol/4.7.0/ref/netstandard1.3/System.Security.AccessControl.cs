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
[assembly: System.Reflection.AssemblyTitle("System.Security.AccessControl")]
[assembly: System.Reflection.AssemblyDescription("System.Security.AccessControl")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.AccessControl")]
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
namespace System.Security.AccessControl
{
    [Flags]
    public enum AccessControlActions
    {
        None = 0,
        View = 1,
        Change = 2
    }

    public enum AccessControlModification
    {
        Add = 0,
        Set = 1,
        Reset = 2,
        Remove = 3,
        RemoveAll = 4,
        RemoveSpecific = 5
    }

    [Flags]
    public enum AccessControlSections
    {
        None = 0,
        Audit = 1,
        Access = 2,
        Owner = 4,
        Group = 8,
        All = 15
    }

    public enum AccessControlType
    {
        Allow = 0,
        Deny = 1
    }

    public abstract partial class AccessRule : AuthorizationRule
    {
        protected AccessRule(Principal.IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default) { }

        public AccessControlType AccessControlType { get { throw null; } }
    }

    public partial class AccessRule<T> : AccessRule where T : struct
    {
        public AccessRule(Principal.IdentityReference identity, T rights, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public AccessRule(Principal.IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public AccessRule(string identity, T rights, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public AccessRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public T Rights { get { throw null; } }
    }

    public sealed partial class AceEnumerator : Collections.IEnumerator
    {
        internal AceEnumerator() { }

        public GenericAce Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    [Flags]
    public enum AceFlags : byte
    {
        None = 0,
        ObjectInherit = 1,
        ContainerInherit = 2,
        NoPropagateInherit = 4,
        InheritOnly = 8,
        InheritanceFlags = 15,
        Inherited = 16,
        SuccessfulAccess = 64,
        FailedAccess = 128,
        AuditFlags = 192
    }

    public enum AceQualifier
    {
        AccessAllowed = 0,
        AccessDenied = 1,
        SystemAudit = 2,
        SystemAlarm = 3
    }

    public enum AceType : byte
    {
        AccessAllowed = 0,
        AccessDenied = 1,
        SystemAudit = 2,
        SystemAlarm = 3,
        AccessAllowedCompound = 4,
        AccessAllowedObject = 5,
        AccessDeniedObject = 6,
        SystemAuditObject = 7,
        SystemAlarmObject = 8,
        AccessAllowedCallback = 9,
        AccessDeniedCallback = 10,
        AccessAllowedCallbackObject = 11,
        AccessDeniedCallbackObject = 12,
        SystemAuditCallback = 13,
        SystemAlarmCallback = 14,
        SystemAuditCallbackObject = 15,
        MaxDefinedAceType = 16,
        SystemAlarmCallbackObject = 16
    }

    [Flags]
    public enum AuditFlags
    {
        None = 0,
        Success = 1,
        Failure = 2
    }

    public abstract partial class AuditRule : AuthorizationRule
    {
        protected AuditRule(Principal.IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags) : base(default!, default, default, default, default) { }

        public AuditFlags AuditFlags { get { throw null; } }
    }

    public partial class AuditRule<T> : AuditRule where T : struct
    {
        public AuditRule(Principal.IdentityReference identity, T rights, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public AuditRule(Principal.IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public AuditRule(string identity, T rights, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public AuditRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(default!, default, default, default, default, default) { }

        public T Rights { get { throw null; } }
    }

    public abstract partial class AuthorizationRule
    {
        protected internal AuthorizationRule(Principal.IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        protected internal int AccessMask { get { throw null; } }

        public Principal.IdentityReference IdentityReference { get { throw null; } }

        public InheritanceFlags InheritanceFlags { get { throw null; } }

        public bool IsInherited { get { throw null; } }

        public PropagationFlags PropagationFlags { get { throw null; } }
    }

    public sealed partial class AuthorizationRuleCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public AuthorizationRule this[int index] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public void AddRule(AuthorizationRule rule) { }

        public void CopyTo(AuthorizationRule[] rules, int index) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class CommonAce : QualifiedAce
    {
        public CommonAce(AceFlags flags, AceQualifier qualifier, int accessMask, Principal.SecurityIdentifier sid, bool isCallback, byte[] opaque) { }

        public override int BinaryLength { get { throw null; } }

        public override void GetBinaryForm(byte[] binaryForm, int offset) { }

        public static int MaxOpaqueLength(bool isCallback) { throw null; }
    }

    public abstract partial class CommonAcl : GenericAcl
    {
        internal CommonAcl() { }

        public sealed override int BinaryLength { get { throw null; } }

        public sealed override int Count { get { throw null; } }

        public bool IsCanonical { get { throw null; } }

        public bool IsContainer { get { throw null; } }

        public bool IsDS { get { throw null; } }

        public sealed override GenericAce this[int index] { get { throw null; } set { } }

        public sealed override byte Revision { get { throw null; } }

        public sealed override void GetBinaryForm(byte[] binaryForm, int offset) { }

        public void Purge(Principal.SecurityIdentifier sid) { }

        public void RemoveInheritedAces() { }
    }

    public abstract partial class CommonObjectSecurity : ObjectSecurity
    {
        protected CommonObjectSecurity(bool isContainer) { }

        protected void AddAccessRule(AccessRule rule) { }

        protected void AddAuditRule(AuditRule rule) { }

        public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType) { throw null; }

        public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType) { throw null; }

        protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified) { throw null; }

        protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified) { throw null; }

        protected bool RemoveAccessRule(AccessRule rule) { throw null; }

        protected void RemoveAccessRuleAll(AccessRule rule) { }

        protected void RemoveAccessRuleSpecific(AccessRule rule) { }

        protected bool RemoveAuditRule(AuditRule rule) { throw null; }

        protected void RemoveAuditRuleAll(AuditRule rule) { }

        protected void RemoveAuditRuleSpecific(AuditRule rule) { }

        protected void ResetAccessRule(AccessRule rule) { }

        protected void SetAccessRule(AccessRule rule) { }

        protected void SetAuditRule(AuditRule rule) { }
    }

    public sealed partial class CommonSecurityDescriptor : GenericSecurityDescriptor
    {
        public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset) { }

        public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, Principal.SecurityIdentifier owner, Principal.SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl) { }

        public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor) { }

        public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm) { }

        public override ControlFlags ControlFlags { get { throw null; } }

        public DiscretionaryAcl DiscretionaryAcl { get { throw null; } set { } }

        public override Principal.SecurityIdentifier Group { get { throw null; } set { } }

        public bool IsContainer { get { throw null; } }

        public bool IsDiscretionaryAclCanonical { get { throw null; } }

        public bool IsDS { get { throw null; } }

        public bool IsSystemAclCanonical { get { throw null; } }

        public override Principal.SecurityIdentifier Owner { get { throw null; } set { } }

        public SystemAcl SystemAcl { get { throw null; } set { } }

        public void AddDiscretionaryAcl(byte revision, int trusted) { }

        public void AddSystemAcl(byte revision, int trusted) { }

        public void PurgeAccessControl(Principal.SecurityIdentifier sid) { }

        public void PurgeAudit(Principal.SecurityIdentifier sid) { }

        public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance) { }

        public void SetSystemAclProtection(bool isProtected, bool preserveInheritance) { }
    }

    public sealed partial class CompoundAce : KnownAce
    {
        public CompoundAce(AceFlags flags, int accessMask, CompoundAceType compoundAceType, Principal.SecurityIdentifier sid) { }

        public override int BinaryLength { get { throw null; } }

        public CompoundAceType CompoundAceType { get { throw null; } set { } }

        public override void GetBinaryForm(byte[] binaryForm, int offset) { }
    }

    public enum CompoundAceType
    {
        Impersonation = 1
    }

    [Flags]
    public enum ControlFlags
    {
        None = 0,
        OwnerDefaulted = 1,
        GroupDefaulted = 2,
        DiscretionaryAclPresent = 4,
        DiscretionaryAclDefaulted = 8,
        SystemAclPresent = 16,
        SystemAclDefaulted = 32,
        DiscretionaryAclUntrusted = 64,
        ServerSecurity = 128,
        DiscretionaryAclAutoInheritRequired = 256,
        SystemAclAutoInheritRequired = 512,
        DiscretionaryAclAutoInherited = 1024,
        SystemAclAutoInherited = 2048,
        DiscretionaryAclProtected = 4096,
        SystemAclProtected = 8192,
        RMControlValid = 16384,
        SelfRelative = 32768
    }

    public sealed partial class CustomAce : GenericAce
    {
        public static readonly int MaxOpaqueLength;
        public CustomAce(AceType type, AceFlags flags, byte[] opaque) { }

        public override int BinaryLength { get { throw null; } }

        public int OpaqueLength { get { throw null; } }

        public override void GetBinaryForm(byte[] binaryForm, int offset) { }

        public byte[] GetOpaque() { throw null; }

        public void SetOpaque(byte[] opaque) { }
    }

    public sealed partial class DiscretionaryAcl : CommonAcl
    {
        public DiscretionaryAcl(bool isContainer, bool isDS, byte revision, int capacity) { }

        public DiscretionaryAcl(bool isContainer, bool isDS, int capacity) { }

        public DiscretionaryAcl(bool isContainer, bool isDS, RawAcl rawAcl) { }

        public void AddAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void AddAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void AddAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, ObjectAccessRule rule) { }

        public bool RemoveAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { throw null; }

        public bool RemoveAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { throw null; }

        public bool RemoveAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, ObjectAccessRule rule) { throw null; }

        public void RemoveAccessSpecific(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void RemoveAccessSpecific(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void RemoveAccessSpecific(AccessControlType accessType, Principal.SecurityIdentifier sid, ObjectAccessRule rule) { }

        public void SetAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void SetAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void SetAccess(AccessControlType accessType, Principal.SecurityIdentifier sid, ObjectAccessRule rule) { }
    }

    public abstract partial class GenericAce
    {
        internal GenericAce() { }

        public AceFlags AceFlags { get { throw null; } set { } }

        public AceType AceType { get { throw null; } }

        public AuditFlags AuditFlags { get { throw null; } }

        public abstract int BinaryLength { get; }

        public InheritanceFlags InheritanceFlags { get { throw null; } }

        public bool IsInherited { get { throw null; } }

        public PropagationFlags PropagationFlags { get { throw null; } }

        public GenericAce Copy() { throw null; }

        public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset) { throw null; }

        public sealed override bool Equals(object o) { throw null; }

        public abstract void GetBinaryForm(byte[] binaryForm, int offset);
        public sealed override int GetHashCode() { throw null; }

        public static bool operator ==(GenericAce left, GenericAce right) { throw null; }

        public static bool operator !=(GenericAce left, GenericAce right) { throw null; }
    }

    public abstract partial class GenericAcl : Collections.ICollection, Collections.IEnumerable
    {
        public static readonly byte AclRevision;
        public static readonly byte AclRevisionDS;
        public static readonly int MaxBinaryLength;
        public abstract int BinaryLength { get; }
        public abstract int Count { get; }

        public bool IsSynchronized { get { throw null; } }

        public abstract GenericAce this[int index] { get; set; }

        public abstract byte Revision { get; }

        public virtual object SyncRoot { get { throw null; } }

        public void CopyTo(GenericAce[] array, int index) { }

        public abstract void GetBinaryForm(byte[] binaryForm, int offset);
        public AceEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public abstract partial class GenericSecurityDescriptor
    {
        public int BinaryLength { get { throw null; } }

        public abstract ControlFlags ControlFlags { get; }
        public abstract Principal.SecurityIdentifier Group { get; set; }
        public abstract Principal.SecurityIdentifier Owner { get; set; }

        public static byte Revision { get { throw null; } }

        public void GetBinaryForm(byte[] binaryForm, int offset) { }

        public string GetSddlForm(AccessControlSections includeSections) { throw null; }

        public static bool IsSddlConversionSupported() { throw null; }
    }

    [Flags]
    public enum InheritanceFlags
    {
        None = 0,
        ContainerInherit = 1,
        ObjectInherit = 2
    }

    public abstract partial class KnownAce : GenericAce
    {
        internal KnownAce() { }

        public int AccessMask { get { throw null; } set { } }

        public Principal.SecurityIdentifier SecurityIdentifier { get { throw null; } set { } }
    }

    public abstract partial class NativeObjectSecurity : CommonObjectSecurity
    {
        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, Runtime.InteropServices.SafeHandle handle, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(default) { }

        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, Runtime.InteropServices.SafeHandle handle, AccessControlSections includeSections) : base(default) { }

        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(default) { }

        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(default) { }

        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections) : base(default) { }

        protected NativeObjectSecurity(bool isContainer, ResourceType resourceType) : base(default) { }

        protected void Persist(Runtime.InteropServices.SafeHandle handle, AccessControlSections includeSections, object exceptionContext) { }

        protected sealed override void Persist(Runtime.InteropServices.SafeHandle handle, AccessControlSections includeSections) { }

        protected void Persist(string name, AccessControlSections includeSections, object exceptionContext) { }

        protected sealed override void Persist(string name, AccessControlSections includeSections) { }

        protected internal delegate Exception ExceptionFromErrorCode(int errorCode, string name, Runtime.InteropServices.SafeHandle handle, object context);
    }

    public abstract partial class ObjectAccessRule : AccessRule
    {
        protected ObjectAccessRule(Principal.IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AccessControlType type) : base(default!, default, default, default, default, default) { }

        public Guid InheritedObjectType { get { throw null; } }

        public ObjectAceFlags ObjectFlags { get { throw null; } }

        public Guid ObjectType { get { throw null; } }
    }

    public sealed partial class ObjectAce : QualifiedAce
    {
        public ObjectAce(AceFlags aceFlags, AceQualifier qualifier, int accessMask, Principal.SecurityIdentifier sid, ObjectAceFlags flags, Guid type, Guid inheritedType, bool isCallback, byte[] opaque) { }

        public override int BinaryLength { get { throw null; } }

        public Guid InheritedObjectAceType { get { throw null; } set { } }

        public ObjectAceFlags ObjectAceFlags { get { throw null; } set { } }

        public Guid ObjectAceType { get { throw null; } set { } }

        public override void GetBinaryForm(byte[] binaryForm, int offset) { }

        public static int MaxOpaqueLength(bool isCallback) { throw null; }
    }

    [Flags]
    public enum ObjectAceFlags
    {
        None = 0,
        ObjectAceTypePresent = 1,
        InheritedObjectAceTypePresent = 2
    }

    public abstract partial class ObjectAuditRule : AuditRule
    {
        protected ObjectAuditRule(Principal.IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AuditFlags auditFlags) : base(default!, default, default, default, default, default) { }

        public Guid InheritedObjectType { get { throw null; } }

        public ObjectAceFlags ObjectFlags { get { throw null; } }

        public Guid ObjectType { get { throw null; } }
    }

    public abstract partial class ObjectSecurity
    {
        protected ObjectSecurity() { }

        protected ObjectSecurity(bool isContainer, bool isDS) { }

        protected ObjectSecurity(CommonSecurityDescriptor securityDescriptor) { }

        public abstract Type AccessRightType { get; }

        protected bool AccessRulesModified { get { throw null; } set { } }

        public abstract Type AccessRuleType { get; }

        public bool AreAccessRulesCanonical { get { throw null; } }

        public bool AreAccessRulesProtected { get { throw null; } }

        public bool AreAuditRulesCanonical { get { throw null; } }

        public bool AreAuditRulesProtected { get { throw null; } }

        protected bool AuditRulesModified { get { throw null; } set { } }

        public abstract Type AuditRuleType { get; }

        protected bool GroupModified { get { throw null; } set { } }

        protected bool IsContainer { get { throw null; } }

        protected bool IsDS { get { throw null; } }

        protected bool OwnerModified { get { throw null; } set { } }

        public abstract AccessRule AccessRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);
        public abstract AuditRule AuditRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);
        public Principal.IdentityReference GetGroup(Type targetType) { throw null; }

        public Principal.IdentityReference GetOwner(Type targetType) { throw null; }

        public byte[] GetSecurityDescriptorBinaryForm() { throw null; }

        public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections) { throw null; }

        public static bool IsSddlConversionSupported() { throw null; }

        protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);
        public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified) { throw null; }

        protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);
        public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified) { throw null; }

        protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections) { }

        protected virtual void Persist(Runtime.InteropServices.SafeHandle handle, AccessControlSections includeSections) { }

        protected virtual void Persist(string name, AccessControlSections includeSections) { }

        public virtual void PurgeAccessRules(Principal.IdentityReference identity) { }

        public virtual void PurgeAuditRules(Principal.IdentityReference identity) { }

        protected void ReadLock() { }

        protected void ReadUnlock() { }

        public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance) { }

        public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance) { }

        public void SetGroup(Principal.IdentityReference identity) { }

        public void SetOwner(Principal.IdentityReference identity) { }

        public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections) { }

        public void SetSecurityDescriptorBinaryForm(byte[] binaryForm) { }

        public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections) { }

        public void SetSecurityDescriptorSddlForm(string sddlForm) { }

        protected void WriteLock() { }

        protected void WriteUnlock() { }
    }

    public abstract partial class ObjectSecurity<T> : NativeObjectSecurity where T : struct
    {
        protected ObjectSecurity(bool isContainer, ResourceType resourceType, Runtime.InteropServices.SafeHandle safeHandle, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(default, default) { }

        protected ObjectSecurity(bool isContainer, ResourceType resourceType, Runtime.InteropServices.SafeHandle safeHandle, AccessControlSections includeSections) : base(default, default) { }

        protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(default, default) { }

        protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections) : base(default, default) { }

        protected ObjectSecurity(bool isContainer, ResourceType resourceType) : base(default, default) { }

        public override Type AccessRightType { get { throw null; } }

        public override Type AccessRuleType { get { throw null; } }

        public override Type AuditRuleType { get { throw null; } }

        public override AccessRule AccessRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) { throw null; }

        public virtual void AddAccessRule(AccessRule<T> rule) { }

        public virtual void AddAuditRule(AuditRule<T> rule) { }

        public override AuditRule AuditRuleFactory(Principal.IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) { throw null; }

        protected internal void Persist(Runtime.InteropServices.SafeHandle handle) { }

        protected internal void Persist(string name) { }

        public virtual bool RemoveAccessRule(AccessRule<T> rule) { throw null; }

        public virtual void RemoveAccessRuleAll(AccessRule<T> rule) { }

        public virtual void RemoveAccessRuleSpecific(AccessRule<T> rule) { }

        public virtual bool RemoveAuditRule(AuditRule<T> rule) { throw null; }

        public virtual void RemoveAuditRuleAll(AuditRule<T> rule) { }

        public virtual void RemoveAuditRuleSpecific(AuditRule<T> rule) { }

        public virtual void ResetAccessRule(AccessRule<T> rule) { }

        public virtual void SetAccessRule(AccessRule<T> rule) { }

        public virtual void SetAuditRule(AuditRule<T> rule) { }
    }

    public sealed partial class PrivilegeNotHeldException : UnauthorizedAccessException
    {
        public PrivilegeNotHeldException() { }

        public PrivilegeNotHeldException(string privilege, Exception inner) { }

        public PrivilegeNotHeldException(string privilege) { }

        public string PrivilegeName { get { throw null; } }
    }

    [Flags]
    public enum PropagationFlags
    {
        None = 0,
        NoPropagateInherit = 1,
        InheritOnly = 2
    }

    public abstract partial class QualifiedAce : KnownAce
    {
        internal QualifiedAce() { }

        public AceQualifier AceQualifier { get { throw null; } }

        public bool IsCallback { get { throw null; } }

        public int OpaqueLength { get { throw null; } }

        public byte[] GetOpaque() { throw null; }

        public void SetOpaque(byte[] opaque) { }
    }

    public sealed partial class RawAcl : GenericAcl
    {
        public RawAcl(byte revision, int capacity) { }

        public RawAcl(byte[] binaryForm, int offset) { }

        public override int BinaryLength { get { throw null; } }

        public override int Count { get { throw null; } }

        public override GenericAce this[int index] { get { throw null; } set { } }

        public override byte Revision { get { throw null; } }

        public override void GetBinaryForm(byte[] binaryForm, int offset) { }

        public void InsertAce(int index, GenericAce ace) { }

        public void RemoveAce(int index) { }
    }

    public sealed partial class RawSecurityDescriptor : GenericSecurityDescriptor
    {
        public RawSecurityDescriptor(byte[] binaryForm, int offset) { }

        public RawSecurityDescriptor(ControlFlags flags, Principal.SecurityIdentifier owner, Principal.SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl) { }

        public RawSecurityDescriptor(string sddlForm) { }

        public override ControlFlags ControlFlags { get { throw null; } }

        public RawAcl DiscretionaryAcl { get { throw null; } set { } }

        public override Principal.SecurityIdentifier Group { get { throw null; } set { } }

        public override Principal.SecurityIdentifier Owner { get { throw null; } set { } }

        public byte ResourceManagerControl { get { throw null; } set { } }

        public RawAcl SystemAcl { get { throw null; } set { } }

        public void SetFlags(ControlFlags flags) { }
    }

    public enum ResourceType
    {
        Unknown = 0,
        FileObject = 1,
        Service = 2,
        Printer = 3,
        RegistryKey = 4,
        LMShare = 5,
        KernelObject = 6,
        WindowObject = 7,
        DSObject = 8,
        DSObjectAll = 9,
        ProviderDefined = 10,
        WmiGuidObject = 11,
        RegistryWow6432Key = 12
    }

    [Flags]
    public enum SecurityInfos
    {
        Owner = 1,
        Group = 2,
        DiscretionaryAcl = 4,
        SystemAcl = 8
    }

    public sealed partial class SystemAcl : CommonAcl
    {
        public SystemAcl(bool isContainer, bool isDS, byte revision, int capacity) { }

        public SystemAcl(bool isContainer, bool isDS, int capacity) { }

        public SystemAcl(bool isContainer, bool isDS, RawAcl rawAcl) { }

        public void AddAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void AddAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void AddAudit(Principal.SecurityIdentifier sid, ObjectAuditRule rule) { }

        public bool RemoveAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { throw null; }

        public bool RemoveAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { throw null; }

        public bool RemoveAudit(Principal.SecurityIdentifier sid, ObjectAuditRule rule) { throw null; }

        public void RemoveAuditSpecific(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void RemoveAuditSpecific(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void RemoveAuditSpecific(Principal.SecurityIdentifier sid, ObjectAuditRule rule) { }

        public void SetAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType) { }

        public void SetAudit(AuditFlags auditFlags, Principal.SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags) { }

        public void SetAudit(Principal.SecurityIdentifier sid, ObjectAuditRule rule) { }
    }
}