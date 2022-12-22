// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Filters
{
    /// <summary>
    /// An <see cref="ICciFilter"/> to remove members marked with
    /// <see cref="T:System.Runtime.CompilerServices.CompilerGeneratedAttribute"/>.
    /// </summary>
    /// <remarks>
    /// This is a hardened version of <see cref="AttributeMarkedFilter"/>. This <see cref="ICciFilter"/> has the
    /// following differences:
    /// <list type="number">
    /// <item>Is specific to <see cref="T:System.Runtime.CompilerServices.CompilerGeneratedAttribute"/>.</item>
    /// <item>Includes property and event accessors despite annotations.</item>
    /// <item>Excludes leftover <see cref="T:System.Runtime.CompilerServices.CompilerGeneratedAttribute"/>s.</item>
    /// </list>
    /// </remarks>
    public class ExcludeCompilerGeneratedCciFilter : ICciFilter
    {
        private const string CompilerGeneratedTypeName = "System.Runtime.CompilerServices.CompilerGeneratedAttribute";

        public virtual bool Include(INamespaceDefinition ns)
        {
            return ns.GetTypes(includeForwards: true).Any(Include);
        }

        public virtual bool Include(ITypeDefinition type)
        {
            return IsNotMarkedWithAttribute(type);
        }

        public virtual bool Include(ITypeDefinitionMember member)
        {
            // Include all accessors. Accessors are marked with CompilerGeneratedAttribute when compiler provides the
            // body e.g. for { get; }.
            if (member is IMethodDefinition methodDefinition &&
                methodDefinition.IsPropertyOrEventAccessor())
            {
                return true;
            }

            return IsNotMarkedWithAttribute(member);
        }

        public virtual bool Include(ICustomAttribute attribute)
        {
            // Include all FakeCustomAttribute because they cannot be annotated and they have simple arguments.
            if (attribute is FakeCustomAttribute)
            {
                return true;
            }

            // Exclude leftover [CompilerGenerated] annotations (should exist only on event and property accessors).
            if (string.Equals(CompilerGeneratedTypeName, attribute.Type.FullName(), StringComparison.Ordinal))
            {
                return false;
            }

            // Exclude attributes with typeof() argument of a compiler-generated type.
            foreach (var arg in attribute.Arguments.OfType<IMetadataTypeOf>())
            {
                var typeDef = arg.TypeToGet.GetDefinitionOrNull();
                if (typeDef != null && !IsNotMarkedWithAttribute(typeDef))
                {
                    return false;
                }
            }

            return IsNotMarkedWithAttribute(attribute.Type.ResolvedType);
        }

        private static bool IsNotMarkedWithAttribute(IReference definition)
        {
            return !definition.Attributes.Any(
                a => string.Equals(a.Type.FullName(), CompilerGeneratedTypeName, StringComparison.Ordinal));
        }
    }
}
