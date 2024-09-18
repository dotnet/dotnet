// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

public class HtmlFormattingFixture : IDisposable
{
    private readonly HtmlFormattingService _htmlFormattingService = new();

    internal HtmlFormattingService Service => _htmlFormattingService;

    public void Dispose()
    {
        _htmlFormattingService.Dispose();
    }
}
