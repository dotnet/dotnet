// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Xunit;
using static Microsoft.AspNetCore.Mvc.Razor.Extensions.ViewComponentsApi;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class TagHelpersIntegrationTest() : IntegrationTestBase(layer: TestProject.Layer.Compiler)
{
    [Fact]
    public void SimpleTagHelpers()
    {
        // Arrange
        TagHelperCollection tagHelpers =
        [
            CreateTagHelperDescriptor(
                tagName: "input",
                typeName: "InputTagHelper",
                assemblyName: "TestAssembly")
        ];

        var projectEngine = CreateProjectEngine(builder => builder.SetTagHelpers(tagHelpers));
        var projectItem = CreateProjectItemFromFile();

        // Act
        var codeDocument = projectEngine.Process(projectItem);

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetRequiredDocumentNode());
    }

    [Fact]
    public void TagHelpersWithBoundAttributes()
    {
        // Arrange
        TagHelperCollection tagHelpers =
        [
            CreateTagHelperDescriptor(
                tagName: "input",
                typeName: "InputTagHelper",
                assemblyName: "TestAssembly",
                attributes:
                [
                    builder => builder
                        .Name("bound")
                        .PropertyName("FooProp")
                        .TypeName("System.String"),
                ])
        ];

        var projectEngine = CreateProjectEngine(builder => builder.SetTagHelpers(tagHelpers));
        var projectItem = CreateProjectItemFromFile();

        // Act
        var codeDocument = projectEngine.Process(projectItem);

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetRequiredDocumentNode());
    }

    [Fact]
    public void NestedTagHelpers()
    {
        // Arrange
        TagHelperCollection tagHelpers =
        [
            CreateTagHelperDescriptor(
                tagName: "p",
                typeName: "PTagHelper",
                assemblyName: "TestAssembly"),
            CreateTagHelperDescriptor(
                tagName: "form",
                typeName: "FormTagHelper",
                assemblyName: "TestAssembly"),
            CreateTagHelperDescriptor(
                tagName: "input",
                typeName: "InputTagHelper",
                assemblyName: "TestAssembly",
                attributes:
                [
                    builder => builder
                        .Name("value")
                        .PropertyName("FooProp")
                        .TypeName("System.String"),
                ])
        ];

        var projectEngine = CreateProjectEngine(builder => builder.SetTagHelpers(tagHelpers));
        var projectItem = CreateProjectItemFromFile();

        // Act
        var codeDocument = projectEngine.Process(projectItem);

        // Assert
        AssertDocumentNodeMatchesBaseline(codeDocument.GetRequiredDocumentNode());
    }

    private static TagHelperDescriptor CreateTagHelperDescriptor(
        string tagName,
        string typeName,
        string assemblyName,
        IEnumerable<Action<BoundAttributeDescriptorBuilder>>? attributes = null)
    {
        var builder = TagHelperDescriptorBuilder.CreateTagHelper(typeName, assemblyName);
        builder.SetTypeName(typeName, typeNamespace: null, typeNameIdentifier: null);

        if (attributes != null)
        {
            foreach (var attributeBuilder in attributes)
            {
                builder.BoundAttributeDescriptor(attributeBuilder);
            }
        }

        builder.TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName(tagName));

        var descriptor = builder.Build();

        return descriptor;
    }
}
