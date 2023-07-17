// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Editor.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class ElementCompletionContextWrapper(ElementCompletionContext obj) : Wrapper<ElementCompletionContext>(obj), IRazorElementCompletionContext
    {
    }
}
