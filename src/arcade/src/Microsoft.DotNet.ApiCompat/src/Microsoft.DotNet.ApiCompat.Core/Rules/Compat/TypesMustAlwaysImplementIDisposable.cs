// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Differs.Rules
{
    // Candidate for strict mode
    //[ExportDifferenceRule]
    internal class TypesMustAlwaysImplementIDisposable : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            if (ImplementsIDisposable(impl) && !ImplementsIDisposable(contract))
            {
                differences.AddIncompatibleDifference(this,
                    $"Type '{impl.FullName()}' implements IDisposable in the {Implementation} but not the {Contract}.");
                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }

        private bool ImplementsIDisposable(ITypeDefinition type)
        {
            foreach (ITypeReference iface in type.Interfaces)
            {
                if (iface.AreEquivalent("System.IDisposable"))
                    return true;
            }
            return false;
        }
    }
}
