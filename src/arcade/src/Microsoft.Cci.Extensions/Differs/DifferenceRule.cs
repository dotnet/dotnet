// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs
{
    public abstract class DifferenceRule : IDifferenceRule
    {
        public virtual DifferenceType Diff<T>(IDifferences differences, ElementMapping<T> mapping) where T : class
        {
            if (mapping.ElementCount < 2)
                return DifferenceType.Unchanged;

            MemberMapping member = mapping as MemberMapping;
            if (member != null)
                return Diff(differences, member);

            TypeMapping type = mapping as TypeMapping;
            if (type != null)
                return Diff(differences, type);

            NamespaceMapping ns = mapping as NamespaceMapping;
            if (ns != null)
                return Diff(differences, ns);

            AssemblyMapping asm = mapping as AssemblyMapping;
            if (asm != null)
                return Diff(differences, asm);

            AssemblySetMapping asmSet = mapping as AssemblySetMapping;
            if (asmSet != null)
                return Diff(differences, asmSet);

            return DifferenceType.Unknown;
        }

        public virtual DifferenceType Diff(IDifferences differences, MemberMapping mapping)
        {
            return Diff(differences, mapping[0], mapping[1]);
        }

        public virtual DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            return DifferenceType.Unknown;
        }

        public virtual DifferenceType Diff(IDifferences differences, TypeMapping mapping)
        {
            return Diff(differences, mapping[0], mapping[1]);
        }

        public virtual DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            return DifferenceType.Unknown;
        }

        public virtual DifferenceType Diff(IDifferences differences, NamespaceMapping mapping)
        {
            return Diff(differences, mapping[0], mapping[1]);
        }

        public virtual DifferenceType Diff(IDifferences differences, INamespaceDefinition impl, INamespaceDefinition contract)
        {
            return DifferenceType.Unknown;
        }

        public virtual DifferenceType Diff(IDifferences differences, AssemblyMapping mapping)
        {
            return Diff(differences, mapping[0], mapping[1]);
        }

        public virtual DifferenceType Diff(IDifferences differences, IAssembly impl, IAssembly contract)
        {
            return DifferenceType.Unknown;
        }

        public virtual DifferenceType Diff(IDifferences differences, AssemblySetMapping mapping)
        {
            return Diff(differences, mapping[0], mapping[1]);
        }

        public virtual DifferenceType Diff(IDifferences differences, IEnumerable<IAssembly> impl, IEnumerable<IAssembly> contract)
        {
            return DifferenceType.Unknown;
        }
    }
}
