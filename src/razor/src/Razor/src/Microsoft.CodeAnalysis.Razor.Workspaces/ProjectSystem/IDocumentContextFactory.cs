// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IDocumentContextFactory
{
    bool TryCreate(
        Uri documentUri,
        VSProjectContext? projectContext,
        [NotNullWhen(true)] out DocumentContext? context);
}
