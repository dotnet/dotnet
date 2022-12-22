// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class AddUsingsCodeActionParams
{
    public required Uri Uri { get; set; }
    public required string Namespace { get; set; }
}
