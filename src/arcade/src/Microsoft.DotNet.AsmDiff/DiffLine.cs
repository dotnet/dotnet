// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class DiffLine
    {
        public DiffLineKind Kind { get; private set; }
        public ReadOnlyCollection<DiffToken> Tokens { get; private set; }

        public DiffLine(DiffLineKind kind, IEnumerable<DiffToken> tokens)
        {
            Kind = kind;
            Tokens = new ReadOnlyCollection<DiffToken>(tokens.ToArray());
        }

        public override string ToString()
        {
            return string.Concat(Tokens);
        }
    }
}
