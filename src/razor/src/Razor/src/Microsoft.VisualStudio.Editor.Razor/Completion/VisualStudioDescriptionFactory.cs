// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

[Shared]
[Export(typeof(IVisualStudioDescriptionFactory))]
internal class VisualStudioDescriptionFactory : IVisualStudioDescriptionFactory
{
    // Internal for testing
    internal static readonly ContainerElement SeparatorElement = new(
        ContainerElementStyle.Wrapped,
        new ClassifiedTextElement(
            new ClassifiedTextRun(PredefinedClassificationNames.Comment, "------------")));

    // Hardcoding the Guid here to avoid a reference to Microsoft.VisualStudio.ImageCatalog.dll
    // that is not present in Visual Studio for Mac
    private static readonly Guid s_imageCatalogGuid = new("{ae27a6b0-e345-4288-96df-5eaf394ee369}");
    private static readonly ImageElement s_propertyGlyph = new(
        new ImageId(s_imageCatalogGuid, 2429), // KnownImageIds.Type = 2429
        "Razor Attribute Glyph");
    private static readonly ClassifiedTextRun s_spaceLiteral = new(PredefinedClassificationNames.Literal, " ");
    private static readonly ClassifiedTextRun s_dotLiteral = new(PredefinedClassificationNames.Literal, ".");

    public ContainerElement CreateClassifiedDescription(AggregateBoundAttributeDescription description)
    {
        if (description is null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        var descriptionElements = new List<object>();
        foreach (var descriptionInfo in description.DescriptionInfos)
        {
            if (descriptionElements.Count > 0)
            {
                descriptionElements.Add(SeparatorElement);
            }

            var returnTypeClassification = PredefinedClassificationNames.Type;
            if (TypeNameStringResolver.TryGetSimpleName(descriptionInfo.ReturnTypeName, out var returnTypeName))
            {
                returnTypeClassification = PredefinedClassificationNames.Keyword;
            }
            else
            {
                returnTypeName = descriptionInfo.ReturnTypeName;
            }

            var tagHelperTypeName = descriptionInfo.TypeName;
            var tagHelperTypeNamePrefix = string.Empty;
            var tagHelperTypeNameProper = tagHelperTypeName;

            var lastDot = tagHelperTypeName.LastIndexOf('.');
            if (lastDot > 0)
            {
                var afterLastDot = lastDot + 1;

                // We're pulling apart the type name so the prefix looks like:
                //
                // Microsoft.AspnetCore.Components.
                tagHelperTypeNamePrefix = tagHelperTypeName[..afterLastDot];

                // And the type name looks like BindBinds
                tagHelperTypeNameProper = tagHelperTypeName[afterLastDot..];
            }

            descriptionElements.Add(
                new ContainerElement(
                    ContainerElementStyle.Wrapped,
                    s_propertyGlyph,
                    new ClassifiedTextElement(
                        new ClassifiedTextRun(returnTypeClassification, returnTypeName),
                        s_spaceLiteral,
                        new ClassifiedTextRun(PredefinedClassificationNames.Literal, tagHelperTypeNamePrefix),
                        new ClassifiedTextRun(PredefinedClassificationNames.Type, tagHelperTypeNameProper),
                        s_dotLiteral,
                        new ClassifiedTextRun(PredefinedClassificationNames.Identifier, descriptionInfo.PropertyName))));

            if (descriptionInfo.Documentation is { } documentation)
            {
                descriptionElements.Add(
                    new ContainerElement(
                        ContainerElementStyle.Wrapped,
                        new ClassifiedTextElement(
                            new ClassifiedTextRun(PredefinedClassificationNames.NaturalLanguage, documentation))));
            }
        }

        return new ContainerElement(ContainerElementStyle.Stacked, descriptionElements);
    }

    private static class PredefinedClassificationNames
    {
        public const string Keyword = "keyword";

        public const string Literal = "literal";

        public const string Type = "Type";

        public const string Identifier = "identifier";

        public const string Comment = "comment";

        public const string NaturalLanguage = "natural language";
    }
}
