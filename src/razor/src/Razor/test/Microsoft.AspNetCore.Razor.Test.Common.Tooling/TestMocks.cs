// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class TestMocks
{
    public static TextLoader CreateTextLoader(string filePath, string text)
    {
        return CreateTextLoader(filePath, SourceText.From(text));
    }

    public static TextLoader CreateTextLoader(string filePath, SourceText text)
    {
        var mock = new StrictMock<TextLoader>();

        var textAndVersion = TextAndVersion.Create(text, VersionStamp.Create(), filePath);

        mock.Setup(x => x.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(textAndVersion);

        return mock.Object;
    }
}
