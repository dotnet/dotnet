// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Differs
{
    public interface IDiffingService
    {
        IEnumerable<SyntaxToken> GetTokenList(IDefinition definition);
    }
}
