// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using FormattingOptions = Microsoft.VisualStudio.LanguageServer.Protocol.FormattingOptions;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal class TestFormattingOptionsProvider(FormattingOptions options) : FormattingOptionsProvider
{
    public static readonly TestFormattingOptionsProvider Default = new(
        new FormattingOptions()
        {
            InsertSpaces = true,
            TabSize = 4,
        });

    public override FormattingOptions? GetOptions(Uri uri) => options;
}
