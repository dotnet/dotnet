// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Cci.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsVisibleOutsideAssembly(this ITypeDefinition type)
        {
            return TypeHelper.IsVisibleOutsideAssembly(type);
        }

        public static bool IsVisibleOutsideAssembly(this INestedTypeDefinition type)
        {
            return MemberExtensions.IsVisibleOutsideAssembly((ITypeDefinitionMember)type);
        }

        public static TypeMemberVisibility GetVisibility(this ITypeDefinition type)
        {
            return TypeHelper.TypeVisibilityAsTypeMemberVisibility(type);
        }

        public static bool IsDummy(this IDefinition def)
        {
            return def is Dummy;
        }

        public static ITypeDefinition GetDefinitionOrNull(this ITypeReference type)
        {
            if (type == null)
                return null;

            ITypeDefinition typeDef = type.ResolvedType;

            if (typeDef.IsDummy())
                return null;

            return typeDef;
        }

        public static IEnumerable<ITypeDefinition> GetBaseTypesAndInterfaces(this ITypeDefinition type)
        {
            foreach (var bc in type.BaseClasses)
            {
                ITypeDefinition baseType = bc.GetDefinitionOrNull();

                if (baseType != null)
                    yield return baseType;
            }

            foreach (var iface in type.Interfaces)
            {
                ITypeDefinition baseType = iface.GetDefinitionOrNull();

                if (baseType != null)
                    yield return baseType;
            }
        }

        public static IEnumerable<ITypeDefinition> GetAllBaseTypes(this ITypeDefinition type)
        {
            var types = type.IsInterface ? type.Interfaces : type.BaseClasses;

            foreach (ITypeReference baseTypeRef in types)
            {
                ITypeDefinition baseType = baseTypeRef.GetDefinitionOrNull();

                if (baseType != null)
                {
                    yield return baseType;

                    foreach (ITypeDefinition nextBaseType in GetAllBaseTypes(baseType))
                        yield return nextBaseType;
                }
            }
        }

        public static IEnumerable<ITypeDefinition> GetAllInterfaces(this ITypeDefinition type)
        {
            foreach (var ifaceRef in type.Interfaces)
            {
                ITypeDefinition iface = ifaceRef.GetDefinitionOrNull();

                if (iface != null)
                {
                    yield return iface;

                    // Get all the base types of the interface.
                    foreach (ITypeDefinition nextIfaceRef in GetAllBaseTypes(iface))
                        yield return nextIfaceRef;
                }
            }

            foreach (var baseType in GetAllBaseTypes(type))
                foreach (var iface in GetAllInterfaces(baseType))
                    yield return iface;
        }

        public static IAssembly GetAssembly(this ITypeDefinition type)
        {
            IUnit unit = TypeHelper.GetDefiningUnit(type);

            IAssembly assembly = unit as IAssembly;
            if (assembly != null)
                return assembly;

            IModule module = unit as IModule;
            if (module != null)
                return module.ContainingAssembly;

            return null;
        }

        public static IAssemblyReference GetAssemblyReference(this ITypeReference type)
        {
            IUnitReference unit = TypeHelper.GetDefiningUnitReference(type);

            IAssemblyReference assembly = unit as IAssemblyReference;
            if (assembly != null)
                return assembly;

            IModuleReference module = unit as IModuleReference;
            if (module != null)
                return module.ContainingAssembly;

            return null;
        }

        public static IAssemblyReference GetAssemblyReference(this IReference reference)
        {
            Contract.Requires(reference != null);
            Contract.Requires(!(reference is Dummy));

            IAssemblyReference assembly = reference as IAssemblyReference;
            if (assembly != null)
                return assembly;

            ITypeReference type = reference as ITypeReference;
            if (type != null)
                return type.GetAssemblyReference();

            ITypeMemberReference member = reference as ITypeMemberReference;
            if (member != null)
                return member.ContainingType.GetAssemblyReference();

            throw new NotSupportedException(string.Format("Unknown IReference '{0}' so we cannot get assembly reference!", reference.GetType().FullName));
        }

        public static bool IsGenericParameter(this ITypeReference type)
        {
            return type is IGenericParameterReference || type is IGenericParameter;
        }

        public static bool IsGenericInstance(this ITypeReference type)
        {
            return type is IGenericTypeInstanceReference;
        }

        public static bool IsWindowsRuntimeAssembly(this IAssemblyReference assembly)
        {
            if (assembly == null)
                return false;

            // ContainsForeignTypes == AssemblyFlag 0x200 == windowsruntime bit
            if (assembly.ContainsForeignTypes)
                return true;

            return false;
        }

        public static bool IsWindowsRuntimeType(this ITypeReference type)
        {
            IAssemblyReference assemblyRef = type.GetAssemblyReference();
            return assemblyRef.IsWindowsRuntimeAssembly();
        }

        public static bool IsFunctionPointer(this ITypeReference type)
        {
            return type is IFunctionPointerTypeReference;
        }

        public static string GetTypeName(this ITypeReference type, bool includeNamespace = true)
        {
            TypeNameFormatter formatter = new TypeNameFormatter();
            NameFormattingOptions options = NameFormattingOptions.OmitTypeArguments | NameFormattingOptions.UseReflectionStyleForNestedTypeNames;
            if (!includeNamespace)
                options |= NameFormattingOptions.OmitContainingNamespace;

            string name = formatter.GetTypeName(type, options);
            return name;
        }
        public static string GetTypeName(this ITypeReference type, NameFormattingOptions options)
        {
            TypeNameFormatter formatter = new TypeNameFormatter();
            string name = formatter.GetTypeName(type, options);
            return name;
        }

        public static INamespaceDefinition GetNamespace(this ITypeDefinition type)
        {
            INamespaceTypeDefinition nsType = type as INamespaceTypeDefinition;
            if (nsType != null)
                return nsType.ContainingNamespace;

            INestedTypeDefinition ntType = type as INestedTypeDefinition;
            if (ntType != null)
                return GetNamespace(ntType.ContainingTypeDefinition);

            return null;
        }

        public static string GetNamespaceName(this ITypeReference type)
        {
            INamespaceTypeReference nsType = type as INamespaceTypeReference;
            if (nsType != null)
                return TypeHelper.GetNamespaceName(nsType.ContainingUnitNamespace, NameFormattingOptions.None);

            INestedTypeReference ntType = type as INestedTypeReference;
            if (ntType != null)
                return GetNamespaceName(ntType.ContainingType);

            return "";
        }

        public static string Name(this ITypeDefinition type)
        {
            INamespaceTypeDefinition nsType = type as INamespaceTypeDefinition;
            if (nsType != null)
                return nsType.Name.Value;

            INestedTypeDefinition nType = type as INestedTypeDefinition;
            if (nType != null)
                return nType.Name.Value;

            throw new NotImplementedException("Called .Name on a currently unsupported type definition!");
        }

        public static string FullName(this IReference reference)
        {
            Contract.Requires(reference != null);

            ITypeReference type = reference as ITypeReference;
            if (type != null)
                return TypeHelper.GetTypeName(type, NameFormattingOptions.TypeParameters);

            ITypeMemberReference member = reference as ITypeMemberReference;
            if (member != null)
                return MemberHelper.GetMemberSignature(member, NameFormattingOptions.TypeParameters | NameFormattingOptions.Signature);

            IUnitNamespaceReference ns = reference as IUnitNamespaceReference;
            if (ns != null)
                return TypeHelper.GetNamespaceName(ns, NameFormattingOptions.None);

            INamedEntity named = reference as INamedEntity;
            if (named != null)
                return named.Name.Value;

            Contract.Assert(false, String.Format("Fell through cases in TypeExtensions.FullName() Type of reference: {0}", reference.GetType()));
            return "<Unknown Reference Type>";
        }

        public static string UniqueId(this IReference reference)
        {
            Contract.Requires(reference != null);

            ITypeReference type = reference as ITypeReference;
            if (type != null)
                return type.DocId();

            ITypeMemberReference member = reference as ITypeMemberReference;
            if (member != null)
                return member.DocId();

            IUnitNamespaceReference ns = reference as IUnitNamespaceReference;
            if (ns != null)
                return ns.DocId();

            IAssemblyReference assembly = reference as IAssemblyReference;
            if (assembly != null)
                return assembly.DocId();

            // Include the hash code as well to make it unique so we can use this for a key
            return "<Unknown Reference Type>" + reference.GetHashCode().ToString();
        }

        public static IEnumerable<INamespaceDefinition> GetAllNamespaces(this IAssembly assembly)
        {
            yield return assembly.NamespaceRoot;

            foreach (var ns in assembly.NamespaceRoot.GetNamespaces(true))
                yield return ns;
        }

        public static IEnumerable<INamespaceDefinition> GetNamespaces(this INamespaceDefinition ns, bool recursive = true)
        {
            foreach (var nestedNs in ns.Members.OfType<INamespaceDefinition>())
            {
                yield return nestedNs;

                if (recursive)
                {
                    foreach (var nn in nestedNs.GetNamespaces(recursive))
                        yield return nn;
                }
            }
        }

        public static IEnumerable<INamespaceTypeDefinition> GetTypes(this INamespaceDefinition ns, bool includeForwards = false)
        {
            IEnumerable<INamespaceTypeDefinition> types;

            if (includeForwards)
            {
                types = ns.Members.Select<INamespaceMember, INamespaceTypeDefinition>(nsMember =>
                    {
                        INamespaceAliasForType nsAlias = nsMember as INamespaceAliasForType;
                        if (nsAlias != null)
                            nsMember = nsAlias.AliasedType.ResolvedType as INamespaceTypeDefinition;

                        return nsMember as INamespaceTypeDefinition;
                    }).Where(t => t != null);
            }
            else
            {
                types = ns.Members.OfType<INamespaceTypeDefinition>();
            }
            return types;
        }

        public static IEnumerable<ITypeDefinitionMember> GetAllMembers(this ITypeDefinition type)
        {
            foreach (var m in type.Members)
                yield return m;

            if (type.IsInterface)
            {
                foreach (var m in GetAllMembersFromInterfaces(type))
                    yield return m;
            }
            else
            {
                foreach (var m in GetAllMembersBaseType(type))
                    yield return m;
            }
        }

        public static IEnumerable<ITypeDefinitionMember> GetAllMembersBaseType(this ITypeDefinition type)
        {
            ITypeReference baseTypeRef = type.BaseClasses.FirstOrDefault();

            if (baseTypeRef == null || TypeHelper.TypesAreEquivalent(baseTypeRef, type.PlatformType.SystemObject))
                yield break;

            ITypeDefinition baseType = baseTypeRef.ResolvedType;

            //Contract.Assert(baseType != Dummy.Type);

            foreach (var m in GetAllMembers(baseType))
                yield return m;
        }

        public static IEnumerable<ITypeDefinitionMember> GetAllMembersFromInterfaces(this ITypeDefinition type)
        {
            foreach (ITypeReference iface in type.Interfaces)
            {
                ITypeDefinition ifaceType = iface.ResolvedType;
                //Contract.Assert(ifaceType != Dummy.Type);

                foreach (var m in ifaceType.Members)
                    yield return m;
            }
        }

        public static bool AreGenericTypeEquivalent(this ITypeReference type, string typeName)
        {
            return type is IGenericTypeInstanceReference genericType && genericType.GenericType.AreEquivalent(typeName);
        }

        public static bool AreEquivalent(this ITypeReference type, string typeName)
        {
            return type.FullName() == typeName;
        }

        public static ITypeReference UnWrap(this ITypeReference reference)
        {
            IPointerTypeReference pointer = reference as IPointerTypeReference;
            if (pointer != null)
                return pointer.TargetType.UnWrap();

            IArrayTypeReference array = reference as IArrayTypeReference;
            if (array != null)
                return array.ElementType.UnWrap();

            IModifiedTypeReference modified = reference as IModifiedTypeReference;
            if (modified != null)
                return modified.UnmodifiedType.UnWrap();

            ISpecializedNestedTypeReference specialized = reference as ISpecializedNestedTypeReference;
            if (specialized != null)
                return specialized.UnspecializedVersion.UnWrap();

            IGenericTypeInstanceReference instantiation = reference as IGenericTypeInstanceReference;
            if (instantiation != null)
                return instantiation.GenericType.UnWrap();

            Contract.Assert(reference is INamedTypeReference
                || reference is INestedTypeReference
                || reference is INamespaceTypeReference
                || reference is IGenericTypeParameterReference
                || reference is IGenericMethodParameterReference
                || reference is IFunctionPointerTypeReference
                || reference is IManagedPointerType,
                string.Format("Unexpected type reference that we may need to unwrap {0}", (reference != null ? reference.GetType().FullName : "null")));

            return reference;
        }

        public static string GetTargetFrameworkMoniker(this IAssembly assembly)
        {
            var tfmAttribute = (from a in assembly.AssemblyAttributes
                                where a.Type.FullName() == "System.Runtime.Versioning.TargetFrameworkAttribute"
                                select a).FirstOrDefault();

            if (tfmAttribute == null)
                return string.Empty;

            var nameArgument = tfmAttribute.Arguments.FirstOrDefault() as IMetadataConstant;
            if (nameArgument == null)
                return string.Empty;

            var name = nameArgument.Value as string;
            if (name == null)
                return string.Empty;

            return name;
        }

        public static bool IsReferenceAssembly(this IAssembly assembly)
        {
            return assembly.AssemblyAttributes.Any(a => a.Type.FullName() == "System.Runtime.CompilerServices.ReferenceAssemblyAttribute");
        }

        private static Dictionary<string, string> s_tokenNames = new Dictionary<string, string>()
        {
            {"b77a5c561934e089", "ECMA"},
            {"b03f5f7f11d50a3a", "DEVDIV"},
            {"7cec85d7bea7798e", "SLPLAT"},
            {"31bf3856ad364e35", "SILVERLIGHT"},
            {"24eec0d8c86cda1e", "PHONE"},
        };

        public static string MapPublicKeyTokenToName(string publicKeyToken)
        {
            string name;
            if (!s_tokenNames.TryGetValue(publicKeyToken.ToLower(), out name))
                name = publicKeyToken;

            return name;
        }

        public static string GetPublicKeyTokenName(this IAssemblyReference assembly)
        {
            return MapPublicKeyTokenToName(assembly.GetPublicKeyToken());
        }

        public static string GetPublicKeyTokenName(this AssemblyIdentity identity)
        {
            return MapPublicKeyTokenToName(identity.GetPublicKeyToken());
        }

        public static string GetPublicKeyToken(this IAssemblyReference assembly)
        {
            return GetPublicKeyToken(assembly.AssemblyIdentity);
        }

        public static string GetPublicKeyToken(this AssemblyIdentity identity)
        {
            return identity.PublicKeyToken.Aggregate("", (s, b) => s += b.ToString("x2"));
        }

        public static bool IsFrameworkAssembly(this AssemblyIdentity identity)
        {
            string token = identity.GetPublicKeyToken();

            string mapped = MapPublicKeyTokenToName(token);

            // If the token is the same then we don't a name for it and thus we don't know about it.
            return token != mapped;
        }

        public static bool IsFacade(this IAssembly assembly)
        {
            if (assembly.GetAllTypes().Any(t => t.Name.Value != "<Module>"))
                return false;

            // Don't assert here -- turns out empty assemblies are a thing.
            // Contract.Assert(assembly.ExportedTypes.Any());

            return assembly.ExportedTypes.Any();
        }

        public static IEnumerable<T> OrderByIdentity<T>(this IEnumerable<T> assemblies)
            where T : IAssemblyReference
        {
            return assemblies.OrderBy(a => a.Name.Value, StringComparer.OrdinalIgnoreCase)
                             .ThenBy(a => a.GetPublicKeyToken(), StringComparer.OrdinalIgnoreCase)
                             .ThenBy(a => a.Version);
        }

        public static IEnumerable<AssemblyIdentity> OrderByIdentity(this IEnumerable<AssemblyIdentity> assemblies)
        {
            return assemblies.OrderBy(a => a.Name.Value, StringComparer.OrdinalIgnoreCase)
                             .ThenBy(a => a.GetPublicKeyToken(), StringComparer.OrdinalIgnoreCase)
                             .ThenBy(a => a.Version);
        }

        public static ApiKind GetApiKind(this ITypeDefinition type)
        {
            return type.IsInterface
                ? ApiKind.Interface
                : type.IsDelegate
                    ? ApiKind.Delegate
                    : type.IsEnum
                        ? ApiKind.Enum
                        : type.IsStruct
                            ? ApiKind.Struct
                            : ApiKind.Class;
        }

        public static bool IsConstructorVisible(this ITypeDefinition type)
        {
            foreach (var method in type.Methods)
            {
                if (method.IsConstructor && method.IsVisibleOutsideAssembly())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsConstructorVisibleToFriendAssemblies(this ITypeDefinition type)
        {
            foreach (var method in type.Methods)
            {
                if (method.IsConstructor && method.IsVisibleToFriendAssemblies())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
