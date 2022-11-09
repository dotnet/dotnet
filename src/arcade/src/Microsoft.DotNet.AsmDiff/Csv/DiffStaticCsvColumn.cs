// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffStaticCsvColumn : DiffCsvColumn
    {
        public DiffStaticCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Static"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(TypeMapping mapping)
        {
            return mapping.Representative.IsStatic
                       ? "Yes"
                       : "No";
        }

        public override string GetValue(MemberMapping mapping)
        {
            return IsStatic(mapping.Representative)
                       ? "Yes"
                       : "No";
        }

        private static bool IsStatic(ITypeDefinitionMember member)
        {
            var f = member as IFieldDefinition;
            if (f != null)
                return f.IsStatic;

            var p = member as IPropertyDefinition;
            if (p != null)
                return p.IsStatic;

            var e = member as IEventDefinition;
            if (e != null)
                return e.Adder.IsStatic || e.Remover.IsStatic;

            var method = (IMethodDefinition)member;
            return method.IsStatic || method.IsStaticConstructor;
        }
    }
}
