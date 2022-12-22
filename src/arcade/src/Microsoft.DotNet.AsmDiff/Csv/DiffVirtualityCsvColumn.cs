// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffVirtualityCsvColumn : DiffCsvColumn
    {
        public DiffVirtualityCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Virtuality"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(TypeMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(MemberMapping mapping)
        {
            var virtuality = GetVirtuality(mapping.Representative);

            switch (virtuality)
            {
                case Virtuality.None:
                    return string.Empty;
                case Virtuality.Virtual:
                    return "Virtual";
                case Virtuality.Abstract:
                    return "Abstract";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Virtuality GetVirtuality(ITypeDefinitionMember member)
        {
            var f = member as IFieldDefinition;
            if (f != null)
                return Virtuality.None;

            var p = member as IPropertyDefinition;
            if (p != null)
            {
                var getter = GetVirtuality(p.Getter);
                var setter = GetVirtuality(p.Setter);

                if (getter == Virtuality.None &&
                    setter == Virtuality.None)
                    return Virtuality.None;

                if (getter == Virtuality.Abstract ||
                    setter == Virtuality.Abstract)
                    return Virtuality.Abstract;

                return Virtuality.Virtual;
            }

            var e = member as IEventDefinition;
            if (e != null)
                return GetVirtuality(e.Adder);

            var method = (IMethodReference)member;
            return GetVirtuality(method);
        }

        private static Virtuality GetVirtuality(IMethodReference methodReference)
        {
            if (methodReference == null || methodReference is Dummy || methodReference.ResolvedMethod is Dummy)
                return Virtuality.None;

            var methodDefinition = methodReference.ResolvedMethod;
            if (methodDefinition.IsVirtual)
                return Virtuality.Virtual;

            if (methodDefinition.IsAbstract)
                return Virtuality.Abstract;

            return Virtuality.None;
        }

        private enum Virtuality
        {
            None,
            Virtual,
            Abstract
        }
    }
}
