// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Roslyn.Text.Adornments;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class AssertExtensions
{
    internal static void AssertExpectedClassification(
        this ClassifiedTextRun run,
        string expectedText,
        string expectedClassificationType,
        ClassifiedTextRunStyle expectedClassificationStyle = ClassifiedTextRunStyle.Plain)
    {
        Assert.Equal(expectedText, run.Text);
        Assert.Equal(expectedClassificationType, run.ClassificationTypeName);
        Assert.Equal(expectedClassificationStyle, run.Style);
    }
}
