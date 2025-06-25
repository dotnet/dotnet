// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class ElementCompletionContextWrapper(ElementCompletionContext obj) : Wrapper<ElementCompletionContext>(obj), IRazorElementCompletionContext
    {
    }
}
