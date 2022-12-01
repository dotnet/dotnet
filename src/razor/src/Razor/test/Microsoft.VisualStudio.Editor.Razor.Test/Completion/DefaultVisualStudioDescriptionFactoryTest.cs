// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Text.Adornments;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

public class DefaultVisualStudioDescriptionFactoryTest : TestBase
{
    public DefaultVisualStudioDescriptionFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void CreateClassifiedDescription_SingleDescription_NoSeparator()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        Assert.DoesNotContain(DefaultVisualStudioDescriptionFactory.SeparatorElement, result.Elements);
    }

    [Fact]
    public void CreateClassifiedDescription_MultipleDescription_Separator()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
            new BoundAttributeDescriptionInfo("TheReturnType2", "TheTypeName2", "ThePropertyName2", "The documentation2"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        Assert.Contains(DefaultVisualStudioDescriptionFactory.SeparatorElement, result.Elements);
    }

    [Fact]
    public void CreateClassifiedDescription_RepresentsReturnType()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.Contains(description.DescriptionInfos[0].ReturnTypeName, flattened);
    }

    [Fact]
    public void CreateClassifiedDescription_RepresentsTypeName()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.Contains(description.DescriptionInfos[0].TypeName, flattened);
    }

    [Fact]
    public void CreateClassifiedDescription_RepresentsPropertyName()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.Contains(description.DescriptionInfos[0].PropertyName, flattened);
    }

    [Fact]
    public void CreateClassifiedDescription_RepresentsDocumentation()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("TheReturnType", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.Contains(description.DescriptionInfos[0].Documentation, flattened);
    }

    [Fact]
    public void CreateClassifiedDescription_CanSimplifyKeywordReturnTypes()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("System.String", "TheTypeName", "ThePropertyName", "The documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.DoesNotContain(description.DescriptionInfos[0].ReturnTypeName, flattened);
        Assert.Contains("string", flattened);
    }

    [Fact]
    public void CreateClassifiedDescription_CanRepresentMultipleDescriptions()
    {
        // Arrange
        var factory = new DefaultVisualStudioDescriptionFactory();
        var description = new AggregateBoundAttributeDescription(new[]
        {
            new BoundAttributeDescriptionInfo("System.String", "TheTypeName", "ThePropertyName", "The documentation"),
            new BoundAttributeDescriptionInfo("System.Int32", "TheSecondTypeName", "TheSecondPropertyName", "The second documentation"),
        });

        // Act
        var result = factory.CreateClassifiedDescription(description);

        // Assert
        var flattened = FlattenToStrings(result);
        Assert.Contains(description.DescriptionInfos[0].TypeName, flattened);
        Assert.Contains(description.DescriptionInfos[1].TypeName, flattened);
        Assert.Contains(description.DescriptionInfos[0].Documentation, flattened);
        Assert.Contains(description.DescriptionInfos[1].Documentation, flattened);
    }

    public IReadOnlyList<string> FlattenToStrings(ContainerElement element)
    {
        var flattenedList = new List<string>();
        foreach (var child in element.Elements)
        {
            switch (child)
            {
                case ContainerElement childContainer:
                    var flattened = FlattenToStrings(childContainer);
                    flattenedList.AddRange(flattened);
                    break;
                case ClassifiedTextElement textElement:
                    foreach (var run in textElement.Runs)
                    {
                        flattenedList.Add(run.Text);
                    }

                    break;
            }
        }

        return flattenedList;
    }
}
