// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffUnsafeCsvColumn : DiffCsvColumn
    {
        public DiffUnsafeCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Unsafe"; }
        }

        public bool? IsUnsafe(ITypeDefinitionMember member)
        {
            var field = member as IFieldDefinition;
            if (field != null)
                return field.Type.IsUnsafeType();

            var method = member as IMethodDefinition;
            if (method != null)
                return method.IsMethodUnsafe();

            var property = member as IPropertyDefinition;
            if (property != null)
                return property.Accessors.Any(a => CSharpCciExtensions.IsMethodUnsafe(a.ResolvedMethod));

            var evnt = member as IEventDefinition;
            if (evnt != null)
                return evnt.Accessors.Any(a => a.ResolvedMethod.IsMethodUnsafe());

            return null;
        }

        public override string GetValue(MemberMapping mapping)
        {
            var isUnsafe = IsUnsafe(mapping.Representative);
            return isUnsafe == null
                       ? null
                       : isUnsafe.Value
                             ? "Yes"
                             : "No";
        }
    }
}
