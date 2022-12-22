// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class TypeCannotChangeClassification : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            string implObjType = GetObjectType(impl);
            string contractObjType = GetObjectType(contract);

            if (implObjType != contractObjType)
            {
                differences.AddIncompatibleDifference(this,
                    $"Type '{impl.FullName()}' is a '{implObjType}' in the {Implementation} but is a '{contractObjType}' in the {Contract}.");

                return DifferenceType.Changed;
            }

            if (contract.Attributes.HasIsReadOnlyAttribute() && !impl.Attributes.HasIsReadOnlyAttribute())
            {
                differences.AddIncompatibleDifference(this,
                    $"Type '{impl.FullName()}' is marked as readonly in the {Contract} so it must also be marked readonly in the {Implementation}.");

                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }

        private string GetObjectType(ITypeDefinition type)
        {
            if (type.IsClass)
                return "class";

            if (type.IsValueType)
            {
                if (type.Attributes.HasIsByRefLikeAttribute())
                    return "ref struct";

                return "struct";
            }

            if (type.IsInterface)
                return "interface";

            if (type.IsDelegate)
                return "delegate";

            throw new System.NotSupportedException(string.Format("Only support types that are class, struct, or interface. {0}", type.GetType()));
        }
    }
}
