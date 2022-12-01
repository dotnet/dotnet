// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Host;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class VisualStudioCompletionBroker : ILanguageService
{
    public abstract bool IsCompletionActive(ITextView textView);
}
