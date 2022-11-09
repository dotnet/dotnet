// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Cci.Differs
{
    public interface IDifferences : IEnumerable<Difference>
    {
        DifferenceType DifferenceType { get; }
        void Add(Difference difference);
    }

    public static class DifferencesExtensions
    {
        public static bool ContainsIncompatibleDifferences(this IDifferences differences)
        {
            if (differences.DifferenceType == DifferenceType.Changed)
            {
                return !differences.OfType<IncompatibleDifference>().Any();
            }
            return true;
        }

        public static void AddIncompatibleDifference(this IDifferences differences, object id, string format, params object[] args)
        {
            if (args.Length == 0)
            {
                differences.Add(new IncompatibleDifference(id, format));
            }
            else
            {
                differences.Add(new IncompatibleDifference(id, string.Format(format, args)));
            }
        }

        public static void AddTypeMismatchDifference(this IDifferences differences, object id, ITypeReference type1, ITypeReference type2, string format, params object[] args)
        {
            if (args.Length == 0)
            {
                differences.Add(new TypeMismatchInCompatibleDifference(id, format, type1, type2));
            }
            else
            {
                differences.Add(new TypeMismatchInCompatibleDifference(id, string.Format(format, args), type1, type2));
            }
        }
    }
}
