// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

internal class TestFormattingOptionsProvider : FormattingOptionsProvider
{
    public static readonly TestFormattingOptionsProvider Default = new(
        new FormattingOptions()
        {
            InsertSpaces = true,
            TabSize = 4,
        });
    private readonly FormattingOptions _options;

    public TestFormattingOptionsProvider(FormattingOptions options)
    {
        _options = options;
    }

    public override FormattingOptions? GetOptions(Uri uri) => _options;
}
