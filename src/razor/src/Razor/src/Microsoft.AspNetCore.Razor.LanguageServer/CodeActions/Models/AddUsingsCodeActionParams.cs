// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class AddUsingsCodeActionParams
{
    public required Uri Uri { get; set; }
    public required string Namespace { get; set; }
    public TextDocumentEdit? AdditionalEdit { get; set; }
}
