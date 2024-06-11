// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;

public class DefaultLSPTagHelperTooltipFactoryTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public void CleanSummaryContent_Markup_ReplacesSeeCrefs()
    {
        // Arrange
        var summary = "Accepts <see cref=\"T:System.Collections.List{System.String}\" />s";

        // Act
        var cleanedSummary = DefaultLSPTagHelperTooltipFactory.CleanSummaryContent(summary);

        // Assert
        Assert.Equal("Accepts `List<System.String>`s", cleanedSummary);
    }

    [Fact]
    public void CleanSummaryContent_Markup_ReplacesSeeAlsoCrefs()
    {
        // Arrange
        var summary = "Accepts <seealso cref=\"T:System.Collections.List{System.String}\" />s";

        // Act
        var cleanedSummary = DefaultLSPTagHelperTooltipFactory.CleanSummaryContent(summary);

        // Assert
        Assert.Equal("Accepts `List<System.String>`s", cleanedSummary);
    }

    [Fact]
    public void CleanSummaryContent_Markup_TrimsSurroundingWhitespace()
    {
        // Arrange
        var summary = @"
            Hello

    World

";

        // Act
        var cleanedSummary = DefaultLSPTagHelperTooltipFactory.CleanSummaryContent(summary);

        // Assert
        Assert.Equal(@"Hello

World", cleanedSummary);
    }

    [Fact]
    public async Task TryCreateTooltip_Markup_NoAssociatedTagHelperDescriptions_ReturnsFalse()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var elementDescription = AggregateBoundElementDescription.Empty;

        // Act
        var markdown = await descriptionFactory.TryCreateTooltipAsync("file.razor", elementDescription, MarkupKind.Markdown, CancellationToken.None);

        // Assert
        Assert.Null(markdown);
    }

    [Fact]
    public async Task TryCreateTooltip_Markup_Element_SingleAssociatedTagHelper_ReturnsTrue()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedTagHelperInfos = new[]
        {
            new BoundElementDescriptionInfo("Microsoft.AspNetCore.SomeTagHelper", "<summary>Uses <see cref=\"T:System.Collections.List{System.String}\" />s</summary>"),
        };
        var elementDescription = new AggregateBoundElementDescription(associatedTagHelperInfos.ToImmutableArray());
        // Act
        var markdown = await descriptionFactory.TryCreateTooltipAsync("file.razor", elementDescription, MarkupKind.Markdown, CancellationToken.None);

        // Assert
        Assert.NotNull(markdown);
        Assert.Equal(@"Microsoft.AspNetCore.**SomeTagHelper**

Uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.Markdown, markdown.Kind);
    }

    [Fact]
    public async Task TryCreateTooltip_Markup_Element_PlainText_NoBold()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedTagHelperInfos = new[]
        {
            new BoundElementDescriptionInfo("Microsoft.AspNetCore.SomeTagHelper", "<summary>Uses <see cref=\"T:System.Collections.List{System.String}\" />s</summary>"),
        };
        var elementDescription = new AggregateBoundElementDescription(associatedTagHelperInfos.ToImmutableArray());

        // Act
        var markdown = await descriptionFactory.TryCreateTooltipAsync("file.razor", elementDescription, MarkupKind.PlainText, CancellationToken.None);

        // Assert
        Assert.NotNull(markdown);
        Assert.Equal(@"Microsoft.AspNetCore.SomeTagHelper

Uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.PlainText, markdown.Kind);
    }

    [Fact]
    public void TryCreateTooltip_Markup_Attribute_PlainText_NoBold()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedAttributeDescriptions = new[]
        {
            new BoundAttributeDescriptionInfo(
                ReturnTypeName: "System.String",
                TypeName: "Microsoft.AspNetCore.SomeTagHelpers.SomeTypeName",
                PropertyName: "SomeProperty",
                Documentation: "<summary>Uses <see cref=\"T:System.Collections.List{System.String}\" />s</summary>")
        };
        var attributeDescription = new AggregateBoundAttributeDescription(associatedAttributeDescriptions.ToImmutableArray());

        // Act
        var result = descriptionFactory.TryCreateTooltip(attributeDescription, MarkupKind.PlainText, out var markdown);

        // Assert
        Assert.True(result);
        Assert.Equal(@"string SomeTypeName.SomeProperty

Uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.PlainText, markdown.Kind);
    }

    [Fact]
    public async Task TryCreateTooltip_Markup_Element_MultipleAssociatedTagHelpers_ReturnsTrue()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedTagHelperInfos = new[]
        {
            new BoundElementDescriptionInfo("Microsoft.AspNetCore.SomeTagHelper", "<summary>\nUses <see cref=\"T:System.Collections.List{System.String}\" />s\n</summary>"),
            new BoundElementDescriptionInfo("Microsoft.AspNetCore.OtherTagHelper", "<summary>\nAlso uses <see cref=\"T:System.Collections.List{System.String}\" />s\n\r\n\r\r</summary>"),
        };
        var elementDescription = new AggregateBoundElementDescription(associatedTagHelperInfos.ToImmutableArray());

        // Act
        var markdown = await descriptionFactory.TryCreateTooltipAsync("file.razor", elementDescription, MarkupKind.Markdown, CancellationToken.None);

        // Assert
        Assert.NotNull(markdown);
        Assert.Equal(@"Microsoft.AspNetCore.**SomeTagHelper**

Uses `List<System.String>`s
---
Microsoft.AspNetCore.**OtherTagHelper**

Also uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.Markdown, markdown.Kind);
    }

    [Fact]
    public void TryCreateTooltip_Markup_Attribute_SingleAssociatedAttribute_ReturnsTrue()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedAttributeDescriptions = new[]
        {
            new BoundAttributeDescriptionInfo(
                ReturnTypeName: "System.String",
                TypeName: "Microsoft.AspNetCore.SomeTagHelpers.SomeTypeName",
                PropertyName: "SomeProperty",
                Documentation: "<summary>Uses <see cref=\"T:System.Collections.List{System.String}\" />s</summary>")
        };
        var attributeDescription = new AggregateBoundAttributeDescription(associatedAttributeDescriptions.ToImmutableArray());

        // Act
        var result = descriptionFactory.TryCreateTooltip(attributeDescription, MarkupKind.Markdown, out var markdown);

        // Assert
        Assert.True(result);
        Assert.Equal(@"**string** SomeTypeName.**SomeProperty**

Uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.Markdown, markdown.Kind);
    }

    [Fact]
    public void TryCreateTooltip_Markup_Attribute_MultipleAssociatedAttributes_ReturnsTrue()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new DefaultLSPTagHelperTooltipFactory(projectManager);
        var associatedAttributeDescriptions = new[]
        {
            new BoundAttributeDescriptionInfo(
                ReturnTypeName: "System.String",
                TypeName: "Microsoft.AspNetCore.SomeTagHelpers.SomeTypeName",
                PropertyName: "SomeProperty",
                Documentation: "<summary>Uses <see cref=\"T:System.Collections.List{System.String}\" />s</summary>"),
            new BoundAttributeDescriptionInfo(
                PropertyName: "AnotherProperty",
                TypeName: "Microsoft.AspNetCore.SomeTagHelpers.AnotherTypeName",
                ReturnTypeName: "System.Boolean?",
                Documentation: "<summary>\nUses <see cref=\"T:System.Collections.List{System.String}\" />s\n</summary>"),
        };
        var attributeDescription = new AggregateBoundAttributeDescription(associatedAttributeDescriptions.ToImmutableArray());

        // Act
        var result = descriptionFactory.TryCreateTooltip(attributeDescription, MarkupKind.Markdown, out var markdown);

        // Assert
        Assert.True(result);
        Assert.Equal(@"**string** SomeTypeName.**SomeProperty**

Uses `List<System.String>`s
---
**Boolean?** AnotherTypeName.**AnotherProperty**

Uses `List<System.String>`s", markdown.Value);
        Assert.Equal(MarkupKind.Markdown, markdown.Kind);
    }
}
