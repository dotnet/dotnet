// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class CannotSealType : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            if (impl.IsEffectivelySealed() && !contract.IsEffectivelySealed())
            {
                differences.AddIncompatibleDifference(this,
                        $"Type '{impl.FullName()}' is {(impl.IsSealed ? "actually (has the sealed modifier)" : "effectively (has a private constructor)")} sealed in the {Implementation} but not sealed in the {Contract}.");

                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }
    }
}
