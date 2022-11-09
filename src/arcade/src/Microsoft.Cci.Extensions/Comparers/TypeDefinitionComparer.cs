// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.Cci.Comparers
{
    public sealed class TypeDefinitionComparer : IComparer<ITypeDefinition>
    {
        public int Compare(ITypeDefinition x, ITypeDefinition y)
        {
            var xName = GetName(x);
            var yName = GetName(y);

            return xName == yName
                       ? x.GenericParameterCount.CompareTo(y.GenericParameterCount)
                       : xName.CompareTo(yName);
        }

        private static string GetName(ITypeReference typeReference)
        {
            return TypeHelper.GetTypeName(typeReference, NameFormattingOptions.OmitContainingNamespace |
                                                         NameFormattingOptions.OmitContainingType);
        }
    }
}
