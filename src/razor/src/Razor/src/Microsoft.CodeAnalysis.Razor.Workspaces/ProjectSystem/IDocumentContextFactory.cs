// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IDocumentContextFactory
{
    DocumentContext? TryCreate(Uri documentUri, VSProjectContext? projectContext, bool versioned);
}
