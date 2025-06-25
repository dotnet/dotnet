// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

[CollectionDefinition(Name)]
public class HtmlFormattingCollection : ICollectionFixture<HtmlFormattingFixture>
{
    public const string Name = nameof(HtmlFormattingCollection);
}
