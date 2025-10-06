// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

internal class ValidateBreakpointRangeParams : VSInternalValidateBreakableRangeParams, ITextDocumentPositionParams
{
    public Position Position
    {
        get { return Range.Start; }
        set { throw new NotImplementedException(); }
    }
}
