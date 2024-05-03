// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class ProjectConfigurationStateSynchronizer
{
    private sealed class Comparer : IEqualityComparer<Work>
    {
        public static readonly Comparer Instance = new();

        private Comparer()
        {
        }

        public bool Equals(Work? x, Work? y)
             => (x, y) switch
             {
                 (Work(var keyX), Work(var keyY)) => keyX == keyY,
                 (null, null) => true,

                 _ => false
             };

        public int GetHashCode(Work obj)
            => obj.ProjectKey.GetHashCode();
    }
}
