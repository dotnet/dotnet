// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Formatting;
using Xunit;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[CollectionDefinition(Name)]
public class HtmlFormattingCollection : ICollectionFixture<HtmlFormattingFixture>
{
    public const string Name = nameof(HtmlFormattingCollection);
}
