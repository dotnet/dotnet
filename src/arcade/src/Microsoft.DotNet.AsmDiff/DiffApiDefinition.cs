// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class DiffApiDefinition
    {
        public string Name { get; private set; }
        public IDefinition Definition { get; private set; }
        public IDefinition Left { get; private set; }
        public IDefinition Right { get; private set; }
        public int StartLine { get; internal set; }
        public int EndLine { get; internal set; }
        public DifferenceType Difference { get; private set; }
        public ReadOnlyCollection<DiffApiDefinition> Children { get; private set; }

        public DiffApiDefinition(IDefinition left, IDefinition right, DifferenceType difference, IList<DiffApiDefinition> children)
        {
            IDefinition representative = left ?? right;
            Name = GetName(representative);
            Definition = representative;
            Left = left;
            Right = right;
            Difference = difference;
            Children = new ReadOnlyCollection<DiffApiDefinition>(children);
        }

        public override string ToString()
        {
            return Difference.ToString().Substring(0, 1) + " " + Definition.UniqueId();
        }

        private static string GetName(object obj)
        {
            var assembly = obj as IAssembly;
            if (assembly != null)
                return GetName(assembly);

            var namespaceDefinition = obj as INamespaceDefinition;
            if (namespaceDefinition != null)
                return GetName(namespaceDefinition);

            var typeMemberReference = obj as ITypeMemberReference;
            if (typeMemberReference != null)
                return GetName(typeMemberReference);

            var typeReference = obj as ITypeReference;
            if (typeReference != null)
                return GetName(typeReference);

            throw new NotImplementedException("Unknown CCI object type: " + obj.GetType());
        }

        private static string GetName(IAssembly assembly)
        {
            return assembly.Name.Value;
        }

        private static string GetName(INamespaceDefinition namespaceName)
        {
            var name = namespaceName.ToString();
            return string.IsNullOrEmpty(name)
                       ? "-"
                       : name;
        }

        private static string GetName(ITypeReference typeReference)
        {
            return TypeHelper.GetTypeName(typeReference, NameFormattingOptions.TypeParameters |
                                                         NameFormattingOptions.OmitContainingNamespace);
        }

        private static string GetName(ITypeMemberReference typeMemberReference)
        {
            string memberSignature = MemberHelper.GetMemberSignature(typeMemberReference, NameFormattingOptions.Signature |
                                                                                       NameFormattingOptions.OmitContainingType |
                                                                                       NameFormattingOptions.OmitContainingNamespace |
                                                                                       NameFormattingOptions.PreserveSpecialNames);

            string returnTypeName = GetReturnTypeName(typeMemberReference);
            return returnTypeName == null
                       ? memberSignature
                       : memberSignature + " : " + returnTypeName;
        }

        private static string GetReturnTypeName(ITypeMemberReference typeMemberReference)
        {
            ITypeDefinitionMember typeDefinitionMember = typeMemberReference.ResolvedTypeDefinitionMember;
            var fieldDefinition = typeDefinitionMember as IFieldDefinition;
            if (fieldDefinition != null)
                return GetName(fieldDefinition.Type);

            var propertyDefinition = typeDefinitionMember as IPropertyDefinition;
            if (propertyDefinition != null)
                return GetName(propertyDefinition.Type);

            var methodDefinition = typeDefinitionMember as IMethodDefinition;
            if (methodDefinition != null && !methodDefinition.IsConstructor && !methodDefinition.IsStaticConstructor)
                return GetName(methodDefinition.Type);

            return null;
        }
    }
}
