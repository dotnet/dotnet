// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

internal static class ComparerExtensions
{
    internal static bool Equals<T>(this IComparer<T> comparer, T x, T y)
    {
        return comparer is IEqualityComparer<T> equalityComparer ? equalityComparer.Equals(x, y) : comparer.Compare(x, y) == 0;
    }
}
