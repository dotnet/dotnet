// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Host;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class VisualStudioRazorParserFactory : ILanguageService
{
    public abstract VisualStudioRazorParser Create(VisualStudioDocumentTracker documentTracker);
}
