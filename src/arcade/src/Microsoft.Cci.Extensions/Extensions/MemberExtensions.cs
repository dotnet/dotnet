// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Extensions
{
    public static class MemberExtensions
    {
        public static bool IsVisibleOutsideAssembly(this ITypeDefinitionMember member)
        {
            return MemberHelper.IsVisibleOutsideAssembly(member);
        }

        public static bool IsVisibleToFriendAssemblies(this ITypeDefinitionMember member)
        {
            // MemberHelper in CCI doesn't have a method like IsVisibleOutsideAssembly(...) to use here. This method
            // has similar behavior except that it returns true for all internal and internal protected members as well
            // as non-sealed private protected members.
            switch (member.Visibility)
            {
                case TypeMemberVisibility.Assembly:
                case TypeMemberVisibility.FamilyOrAssembly:
                case TypeMemberVisibility.Public:
                    return true;

                case TypeMemberVisibility.Family:
                case TypeMemberVisibility.FamilyAndAssembly:
                    return !member.ContainingTypeDefinition.IsSealed;
            }

            var containingType = member.ContainingTypeDefinition;
            return member switch
            {
                IMethodDefinition methodDefinition =>
                    IsExplicitImplementationVisible(methodDefinition, containingType),
                IPropertyDefinition propertyDefinition =>
                    IsExplicitImplementationVisible(propertyDefinition.Getter, containingType) ||
                        IsExplicitImplementationVisible(propertyDefinition.Setter, containingType),
                IEventDefinition eventDefinition =>
                    IsExplicitImplementationVisible(eventDefinition.Adder, containingType) ||
                        IsExplicitImplementationVisible(eventDefinition.Remover, containingType),
                _ => false,
            };
        }

        public static bool IsGenericInstance(this IMethodReference method)
        {
            return method is IGenericMethodInstanceReference;
        }

        public static bool IsWindowsRuntimeMember(this ITypeMemberReference member)
        {
            var assemblyRef = member.GetAssemblyReference();

            return assemblyRef.IsWindowsRuntimeAssembly();
        }

        public static string FullName(this ICustomAttribute attribute)
        {
            if (attribute is FakeCustomAttribute fca)
            {
                return fca.FullTypeName;
            }

            return attribute.Type.FullName();
        }

        public static string GetMethodSignature(this IMethodDefinition method)
        {
            return MemberHelper.GetMethodSignature(method, NameFormattingOptions.Signature);
        }

        public static T UnWrapMember<T>(this T member)
           where T : ITypeMemberReference
        {
            return member switch
            {
                IGenericMethodInstanceReference genericMethod => (T)genericMethod.GenericMethod.UnWrapMember(),
                ISpecializedNestedTypeReference type => (T)type.UnspecializedVersion.UnWrapMember(),
                ISpecializedMethodReference method => (T)method.UnspecializedVersion.UnWrapMember(),
                ISpecializedFieldReference field => (T)field.UnspecializedVersion.UnWrapMember(),
                ISpecializedPropertyDefinition property => (T)property.UnspecializedVersion.UnWrapMember(),
                ISpecializedEventDefinition evnt => (T)evnt.UnspecializedVersion.UnWrapMember(),
                _ => member
            };
        }

        public static bool IsPropertyOrEventAccessor(this IMethodDefinition method)
        {
            return method.GetAccessorType() != AccessorType.None;
        }

        public static AccessorType GetAccessorType(this IMethodDefinition methodDefinition)
        {
            if (!methodDefinition.IsSpecialName)
            {
                return AccessorType.None;
            }

            // Cannot use MemberHelper.IsAdder(...) and similar due to their TypeMemberVisibility.Public restriction.
            var name = methodDefinition.GetNameWithoutExplicitType();
            if (name.StartsWith("get_"))
            {
                return AccessorType.PropertyGetter;
            }
            else if (name.StartsWith("set_"))
            {
                return AccessorType.PropertySetter;
            }
            else if (name.StartsWith("add_"))
            {
                return AccessorType.EventAdder;
            }
            else if (name.StartsWith("remove_"))
            {
                return AccessorType.EventRemover;
            }

            return AccessorType.None;
        }

        public static bool IsEditorBrowseableStateNever(this ICustomAttribute attribute)
        {
            if (attribute.Type.FullName() != typeof(EditorBrowsableAttribute).FullName)
            {
                return false;
            }

            if (attribute.Arguments == null || attribute.Arguments.Count() != 1)
            {
                return false;
            }

            var singleArgument = attribute.Arguments.Single() as IMetadataConstant;
            if (singleArgument == null || !(singleArgument.Value is int))
            {
                return false;
            }

            if (EditorBrowsableState.Never != (EditorBrowsableState)singleArgument.Value)
            {
                return false;
            }

            return true;
        }

        public static bool IsObsoleteWithUsageTreatedAsCompilationError(this ICustomAttribute attribute)
        {
            if (attribute.Type.FullName() != typeof(ObsoleteAttribute).FullName)
            {
                return false;
            }

            if (attribute.Arguments == null || attribute.Arguments.Count() != 2)
            {
                return false;
            }

            var messageArgument = attribute.Arguments.ElementAt(0) as IMetadataConstant;
            var errorArgument = attribute.Arguments.ElementAt(1) as IMetadataConstant;
            if (messageArgument == null || errorArgument == null)
            {
                return false;
            }

            if (!(messageArgument.Value is string && errorArgument.Value is bool))
            {
                return false;
            }

            return (bool)errorArgument.Value;
        }

        public static ApiKind GetApiKind(this ITypeDefinitionMember member)
        {
            if (member.ContainingTypeDefinition.IsDelegate)
                return ApiKind.DelegateMember;

            switch (member)
            {
                case IFieldDefinition field:
                    if (member.ContainingTypeDefinition.IsEnum && field.IsSpecialName)
                    {
                        return ApiKind.EnumField;
                    }

                    return ApiKind.Field;

                case IPropertyDefinition _:
                    return ApiKind.Property;

                case IEventDefinition _:
                    return ApiKind.Event;
            }


            var method = (IMethodDefinition)member;
            if (method.IsConstructor || method.IsStaticConstructor)
            {
                return ApiKind.Constructor;
            }

            var accessorType = method.GetAccessorType();
            switch (accessorType)
            {
                case AccessorType.PropertyGetter:
                case AccessorType.PropertySetter:
                    return ApiKind.PropertyAccessor;

                case AccessorType.EventAdder:
                case AccessorType.EventRemover:
                    return ApiKind.EventAccessor;

                default:
                    return ApiKind.Method;
            }
        }

        // A rewrite of MemberHelper.IsExplicitImplementationVisible(...) with looser visibility checks.
        private static bool IsExplicitImplementationVisible(IMethodReference method, ITypeDefinition containingType)
        {
            if (method == null)
            {
                return false;
            }

            using var enumerator = containingType.ExplicitImplementationOverrides.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current.ImplementingMethod.InternedKey == method.InternedKey)
                {
                    var resolvedMethod = current.ImplementedMethod.ResolvedMethod;
                    if (resolvedMethod is Dummy)
                    {
                        return true;
                    }

                    // Reviewers: Recursive call that mimics MemberHelper methods. But, why is this safe? (I'm undoing
                    // my InternalsAndPublicCciFilter.SimpleInclude(...) failsafe but double-checking.)
                    if (IsVisibleToFriendAssemblies(resolvedMethod))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
