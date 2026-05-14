// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Extensions.Experimental
{
    public class OnlyPublicFilterAssembly : IFilterAssembly
    {
        public bool FilterNamespace(INamespaceMember member)
        {
            ITypeDefinition type = member as ITypeDefinition;
            if (type != null)
                return FilterType(type);

            // Lets not filter anything else here yet.
            return false;
        }

        public bool FilterType(ITypeDefinition type)
        {
            if (type.IsVisibleOutsideAssembly())
                return false;
            return true;
        }

        public bool FilterMember(ITypeDefinitionMember member)
        {
            if (member.IsVisibleOutsideAssembly())
                return false;
            return true;
        }
    }
    public interface IFilterAssembly
    {
        bool FilterNamespace(INamespaceMember member);
        bool FilterType(ITypeDefinition type);
        bool FilterMember(ITypeDefinitionMember member);
    }

    //public class FilteredAssembly : IAssembly
    //{
    //    public IAssembly _assembly;
    //    public IFilterAssembly _filter;

    //    public FilteredAssembly(IAssembly assembly, IFilterAssembly filter)
    //    {
    //        _assembly = assembly;
    //        _filter = filter;
    //    }

    //    public IEnumerable<ICustomAttribute> AssemblyAttributes
    //    {
    //        get { return _assembly.AssemblyAttributes; }
    //    }

    //    public IEnumerable<IAliasForType> ExportedTypes
    //    {
    //        get { return _assembly.ExportedTypes; }
    //    }

    //    public IEnumerable<IFileReference> Files
    //    {
    //        get { return _assembly.Files; }
    //    }

    //    public uint Flags
    //    {
    //        get { return _assembly.Flags; }
    //    }

    //    public IEnumerable<IModule> MemberModules
    //    {
    //        get { return _assembly.MemberModules; }
    //    }

    //    public IEnumerable<byte> PublicKey
    //    {
    //        get { return _assembly.PublicKey; }
    //    }

    //    public IEnumerable<IResourceReference> Resources
    //    {
    //        get { return _assembly.Resources; }
    //    }

    //    public IEnumerable<ISecurityAttribute> SecurityAttributes
    //    {
    //        get { return _assembly.SecurityAttributes; }
    //    }

    //    public IEnumerable<IAssemblyReference> AssemblyReferences
    //    {
    //        get { return _assembly.AssemblyReferences; }
    //    }

    //    public ulong BaseAddress
    //    {
    //        get { return _assembly.BaseAddress; }
    //    }

    //    public IAssembly ContainingAssembly
    //    {
    //        get { return _assembly.ContainingAssembly; }
    //    }

    //    public ushort DllCharacteristics
    //    {
    //        get { return _assembly.DllCharacteristics; }
    //    }

    //    public IMethodReference EntryPoint
    //    {
    //        get { return _assembly.EntryPoint; }
    //    }

    //    public uint FileAlignment
    //    {
    //        get { return _assembly.FileAlignment; }
    //    }

    //    public IEnumerable<INamedTypeDefinition> GetAllTypes()
    //    {
    //        return _assembly.GetAllTypes().Where(t => !_filter.FilterType(t));
    //    }

    //    public IEnumerable<string> GetStrings()
    //    {
    //        return _assembly.GetStrings();
    //    }

    //    public bool ILOnly
    //    {
    //        get { return _assembly.ILOnly; }
    //    }

    //    public ModuleKind Kind
    //    {
    //        get { return _assembly.Kind; }
    //    }

    //    public byte LinkerMajorVersion
    //    {
    //        get { return _assembly.LinkerMajorVersion; }
    //    }

    //    public byte LinkerMinorVersion
    //    {
    //        get { return _assembly.LinkerMinorVersion; }
    //    }

    //    public Machine Machine
    //    {
    //        get { return _assembly.Machine; }
    //    }

    //    public byte MetadataFormatMajorVersion
    //    {
    //        get { return _assembly.MetadataFormatMajorVersion; }
    //    }

    //    public byte MetadataFormatMinorVersion
    //    {
    //        get { return _assembly.MetadataFormatMinorVersion; }
    //    }

    //    public IEnumerable<ICustomAttribute> ModuleAttributes
    //    {
    //        get { return _assembly.ModuleAttributes; }
    //    }

    //    public IName ModuleName
    //    {
    //        get { return _assembly.ModuleName; }
    //    }

    //    public IEnumerable<IModuleReference> ModuleReferences
    //    {
    //        get { return _assembly.ModuleReferences; }
    //    }

    //    public Guid PersistentIdentifier
    //    {
    //        get { return _assembly.PersistentIdentifier; }
    //    }

    //    public bool Requires32bits
    //    {
    //        get { return _assembly.Requires32bits; }
    //    }

    //    public bool Requires64bits
    //    {
    //        get { return _assembly.Requires64bits; }
    //    }

    //    public bool RequiresAmdInstructionSet
    //    {
    //        get { return _assembly.RequiresAmdInstructionSet; }
    //    }

    //    public bool RequiresStartupStub
    //    {
    //        get { return _assembly.RequiresStartupStub; }
    //    }

    //    public ulong SizeOfHeapCommit
    //    {
    //        get { return _assembly.SizeOfHeapCommit; }
    //    }

    //    public ulong SizeOfHeapReserve
    //    {
    //        get { return _assembly.SizeOfHeapReserve; }
    //    }

    //    public ulong SizeOfStackCommit
    //    {
    //        get { return _assembly.SizeOfStackCommit; }
    //    }

    //    public ulong SizeOfStackReserve
    //    {
    //        get { return _assembly.SizeOfStackReserve; }
    //    }

    //    public string TargetRuntimeVersion
    //    {
    //        get { return _assembly.TargetRuntimeVersion; }
    //    }

    //    public bool TrackDebugData
    //    {
    //        get { return _assembly.TrackDebugData; }
    //    }

    //    public bool UsePublicKeyTokensForAssemblyReferences
    //    {
    //        get { return _assembly.UsePublicKeyTokensForAssemblyReferences; }
    //    }

    //    public IEnumerable<IWin32Resource> Win32Resources
    //    {
    //        get { return _assembly.Win32Resources; }
    //    }

    //    public AssemblyIdentity ContractAssemblySymbolicIdentity
    //    {
    //        get { return _assembly.ContractAssemblySymbolicIdentity; }
    //    }

    //    public AssemblyIdentity CoreAssemblySymbolicIdentity
    //    {
    //        get { return _assembly.CoreAssemblySymbolicIdentity; }
    //    }

    //    public string Location
    //    {
    //        get { return _assembly.Location; }
    //    }

    //    public IPlatformType PlatformType
    //    {
    //        get { return _assembly.PlatformType; }
    //    }

    //    public IRootUnitNamespace UnitNamespaceRoot
    //    {
    //        get { return _assembly.UnitNamespaceRoot; }
    //    }

    //    public IEnumerable<IUnitReference> UnitReferences
    //    {
    //        get { return _assembly.UnitReferences; }
    //    }

    //    public INamespaceDefinition NamespaceRoot
    //    {
    //        get { return _assembly.NamespaceRoot; }
    //    }

    //    public IUnit ResolvedUnit
    //    {
    //        get { return this; }
    //    }

    //    public UnitIdentity UnitIdentity
    //    {
    //        get { return _assembly.UnitIdentity; }
    //    }

    //    public IEnumerable<ICustomAttribute> Attributes
    //    {
    //        get { return _assembly.Attributes; }
    //    }

    //    public void Dispatch(IMetadataVisitor visitor)
    //    {
    //        _assembly.Dispatch(visitor);
    //    }

    //    public IEnumerable<ILocation> Locations
    //    {
    //        get { return _assembly.Locations; }
    //    }

    //    public IName Name
    //    {
    //        get { return _assembly.Name; }
    //    }

    //    IAssemblyReference IModuleReference.ContainingAssembly
    //    {
    //        get { return _assembly.ContainingAssembly; }
    //    }

    //    public ModuleIdentity ModuleIdentity
    //    {
    //        get { return _assembly.ModuleIdentity; }
    //    }

    //    public IModule ResolvedModule
    //    {
    //        get { return this; }
    //    }

    //    public IEnumerable<IName> Aliases
    //    {
    //        get { return _assembly.Aliases; }
    //    }

    //    public AssemblyIdentity AssemblyIdentity
    //    {
    //        get { return _assembly.AssemblyIdentity; }
    //    }

    //    public string Culture
    //    {
    //        get { return _assembly.Culture; }
    //    }

    //    public bool IsRetargetable
    //    {
    //        get { return _assembly.IsRetargetable; }
    //    }

    //    public IEnumerable<byte> PublicKeyToken
    //    {
    //        get { return _assembly.PublicKeyToken; }
    //    }

    //    public IAssembly ResolvedAssembly
    //    {
    //        get { return this; }
    //    }

    //    public AssemblyIdentity UnifiedAssemblyIdentity
    //    {
    //        get { return _assembly.UnifiedAssemblyIdentity; }
    //    }

    //    public Version Version
    //    {
    //        get { return _assembly.Version; }
    //    }


    //    public void DispatchAsReference(IMetadataVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public bool ContainsForeignTypes
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public IEnumerable<ITypeMemberReference> GetTypeMemberReferences()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerable<ITypeReference> GetTypeReferences()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerable<byte> HashValue
    //    {
    //        get { throw new NotImplementedException(); }
    //    }
    //}

    //public class FilteredRootNamespace : FilteredNamespace, IRootUnitNamespace
    //{
    //    public FilteredRootNamespace(IRootUnitNamespace ns, IFilterAssembly filter)
    //        : base(ns, filter)
    //    {
    //    }

    //    public new IUnitReference Unit
    //    {
    //        get { throw new NotImplementedException(); }
    //    }
    //}

    public class FilteredNamespace : IUnitNamespace
    {
        private IUnitNamespace _ns;
        private IFilterAssembly _filter;

        public FilteredNamespace(IUnitNamespace ns, IFilterAssembly filter)
        {
            _ns = ns;
            _filter = filter;
        }

        public IUnit Unit
        {
            get { return _ns.Unit; }
        }

        public IEnumerable<INamespaceMember> Members
        {
            get { return _ns.Members.Where(m => !_filter.FilterNamespace(m)); }
        }

        public INamespaceRootOwner RootOwner
        {
            get { return _ns.RootOwner; }
        }

        public IEnumerable<ICustomAttribute> Attributes
        {
            get { return _ns.Attributes; }
        }

        public void Dispatch(IMetadataVisitor visitor)
        {
            _ns.Dispatch(visitor);
        }

        public IEnumerable<ILocation> Locations
        {
            get { return _ns.Locations; }
        }

        public IName Name
        {
            get { return _ns.Name; }
        }

        public bool Contains(INamespaceMember member)
        {
            return this.Members.Contains(member);
        }

        public IEnumerable<INamespaceMember> GetMatchingMembers(Function<INamespaceMember, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INamespaceMember> GetMatchingMembersNamed(IName name, bool ignoreCase, Function<INamespaceMember, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INamespaceMember> GetMembersNamed(IName name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public IUnitNamespace ResolvedUnitNamespace
        {
            get { return this; }
        }

        IUnitReference IUnitNamespaceReference.Unit
        {
            get { return _ns.Unit; }
        }


        public void DispatchAsReference(IMetadataVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }

    //public class FilteredNamespaceTypeDefinition : FilteredTypeDefinition, INamespaceTypeDefinition
    //{

    //}

    public class FilteredTypeDefinition : ITypeDefinition
    {
        public ushort Alignment
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ITypeReference> BaseClasses
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IEventDefinition> Events
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IMethodImplementation> ExplicitImplementationOverrides
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IFieldDefinition> Fields
        {
            get { throw new NotImplementedException(); }
        }

        public ushort GenericParameterCount
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IGenericTypeParameter> GenericParameters
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasDeclarativeSecurity
        {
            get { throw new NotImplementedException(); }
        }

        public IGenericTypeInstanceReference InstanceType
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ITypeReference> Interfaces
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAbstract
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsBeforeFieldInit
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsClass
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsComObject
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDelegate
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGeneric
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInterface
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReferenceType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRuntimeSpecial
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSealed
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSerializable
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSpecialName
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsStruct
        {
            get { throw new NotImplementedException(); }
        }

        public LayoutKind Layout
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ITypeDefinitionMember> Members
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IMethodDefinition> Methods
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<INestedTypeDefinition> NestedTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ITypeDefinitionMember> PrivateHelperMembers
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IPropertyDefinition> Properties
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ISecurityAttribute> SecurityAttributes
        {
            get { throw new NotImplementedException(); }
        }

        public uint SizeOf
        {
            get { throw new NotImplementedException(); }
        }

        public StringFormatKind StringFormat
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeReference UnderlyingType
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<ICustomAttribute> Attributes
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispatch(IMetadataVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILocation> Locations
        {
            get { throw new NotImplementedException(); }
        }

        public bool Contains(ITypeDefinitionMember member)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITypeDefinitionMember> GetMatchingMembers(Function<ITypeDefinitionMember, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITypeDefinitionMember> GetMatchingMembersNamed(IName name, bool ignoreCase, Function<ITypeDefinitionMember, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITypeDefinitionMember> GetMembersNamed(IName name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public IAliasForType AliasForType
        {
            get { throw new NotImplementedException(); }
        }

        public uint InternedKey
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAlias
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsEnum
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsValueType
        {
            get { throw new NotImplementedException(); }
        }

        public IPlatformType PlatformType
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeDefinition ResolvedType
        {
            get { throw new NotImplementedException(); }
        }

        public PrimitiveTypeCode TypeCode
        {
            get { throw new NotImplementedException(); }
        }


        public void DispatchAsReference(IMetadataVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
