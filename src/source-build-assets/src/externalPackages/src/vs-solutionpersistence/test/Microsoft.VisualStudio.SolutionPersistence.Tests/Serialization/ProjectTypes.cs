// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

namespace Serialization;

/// <summary>
/// Tests related to project types in the model.
/// </summary>
public sealed class ProjectTypes
{
    /// <summary>
    /// Keeps the checked in built-in project types test in sync with the code version.
    /// </summary>
    [Fact]
    public async Task BuiltInProjectTypes()
    {
        // Turn the build-in project types into a solution model.
        SolutionModel builtInTypesModel = new SolutionModel
        {
            ProjectTypes = ProjectTypeTable.BuiltInTypes.ProjectTypes,
            SerializerExtension = SolutionSerializers.SlnXml.CreateModelExtension(
                new SlnxSerializerSettings()
                {
                    // Use 4 spaces for checked in built-in slnx.
                    IndentChars = "    ",
                }),
        };

        FileContents builtInFromCodeLines = await builtInTypesModel.ToLinesAsync(SolutionSerializers.SlnXml);

        AssertSolutionsAreEqual(builtInFromCodeLines, SlnAssets.XmlBuiltInProjectTypes.ToLines());
    }

    /// <summary>
    /// Attempt to set project types to invalid values.
    /// </summary>
    [Fact]
    public void InvalidTypes()
    {
    }
}
