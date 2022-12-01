// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class CodeActionResolveParams
{
    public object? Data { get; set; }
    public required Uri RazorFileUri { get; set; }
}
