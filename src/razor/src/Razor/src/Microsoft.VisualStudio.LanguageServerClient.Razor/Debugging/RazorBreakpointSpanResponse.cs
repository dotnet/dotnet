// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

internal class RazorBreakpointSpanResponse
{
    public required Range Range { get; init; }
}
