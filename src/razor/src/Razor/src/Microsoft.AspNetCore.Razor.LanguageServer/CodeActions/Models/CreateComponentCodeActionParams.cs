// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class CreateComponentCodeActionParams
{
    public required Uri Uri { get; set; }
    public required string Path { get; set; }
}
