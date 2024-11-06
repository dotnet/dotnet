// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Text.Adornments;
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
