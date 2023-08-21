// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal class GenerateMethodCodeActionParams
{
    public required Uri Uri { get; set; }
    public required string MethodName { get; set; }
    public required string EventName { get; set;}
    public required bool IsAsync { get; set; }
}
