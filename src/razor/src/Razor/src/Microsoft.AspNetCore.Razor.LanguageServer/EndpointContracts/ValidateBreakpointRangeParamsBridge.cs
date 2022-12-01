// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

internal class ValidateBreakpointRangeParamsBridge : VSInternalValidateBreakableRangeParams, ITextDocumentPositionParams
{
    public Position Position
    {
        get { return Range.Start; }
        set { throw new NotImplementedException(); }
    }
}
