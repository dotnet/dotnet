// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Serialization;

/// <summary>
/// Tests for generated model guid ids.
/// </summary>
public sealed class IdGenerator
{
    /// <summary>
    /// Checks that the generated ids are deterministic and unique.
    /// Since there are different id implementations based on the runtime,
    /// this test ensures that the different algorithms don't diverge.
    /// </summary>
    [Fact]
    public void CheckGeneratedIds()
    {
        Guid parentId1 = new Guid("d5a52153-8a08-41d3-8c37-6b19822216a1");
        Guid parentId2 = new Guid("d5a52153-8a08-41d3-8c37-6b19822216a2");

        Assert.Equal(Guid.Empty, DefaultIdGenerator.CreateIdFrom(string.Empty));

        Assert.Equal(Guid.Empty, DefaultIdGenerator.CreateIdFrom(Guid.Empty, string.Empty));

        // If there is no name, the id no-ops.
        Assert.Equal(Guid.Empty, DefaultIdGenerator.CreateIdFrom(parentId1, string.Empty));

        // This uses SHA256, so the result should be deterministic on different platforms and runtimes.
        Assert.Equal(
            new Guid("18919072-e206-b539-8a9a-dca53cd56706"),
            DefaultIdGenerator.CreateIdFrom("/FolderName/"));

        // This uses SHA256, so the result should be deterministic on different platforms and runtimes.
        Assert.Equal(
            new Guid("b66dccfb-7b96-516f-acdd-b917e28a7b4d"),
            DefaultIdGenerator.CreateIdFrom(parentId1, "/FolderName/"));

        // This uses SHA256, so the result should be deterministic on different platforms and runtimes.
        Assert.Equal(
            new Guid("fa90428c-090a-5f72-9eda-605911609bd0"),
            DefaultIdGenerator.CreateIdFrom(parentId1, "Item"));

        // This uses SHA256, so the result should be deterministic on different platforms and runtimes.
        Assert.Equal(
            new Guid("86165904-0d97-7c1f-db13-5ff23046c267"),
            DefaultIdGenerator.CreateIdFrom(parentId1, Path.Join("Item", "Item")));

        Assert.NotEqual(
            DefaultIdGenerator.CreateIdFrom("Item1"),
            DefaultIdGenerator.CreateIdFrom("Item2"));

        Assert.NotEqual(
            DefaultIdGenerator.CreateIdFrom(parentId1, "Item1"),
            DefaultIdGenerator.CreateIdFrom(parentId2, "Item1"));

        Assert.NotEqual(
            DefaultIdGenerator.CreateIdFrom(parentId1, "Item1"),
            DefaultIdGenerator.CreateIdFrom(parentId1, "Item2"));

        // Case insensitive.
        Assert.Equal(
            DefaultIdGenerator.CreateIdFrom("ITEM1"),
            DefaultIdGenerator.CreateIdFrom("iTeM1"));

        // Slash insensitive.
        Assert.Equal(
            DefaultIdGenerator.CreateIdFrom("Item1\\Item2"),
            DefaultIdGenerator.CreateIdFrom("Item1/Item2"));
    }

    /// <summary>
    /// Verify that serialized model ids are deterministic and unique.
    /// </summary>
    [Fact]
    public async Task CheckModelIds()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        SolutionModel solution = await SolutionSerializers.SlnXml.OpenAsync(SlnAssets.XmlSlnxMany.Stream, CancellationToken.None);

        SolutionProjectModel? urlProject = solution.FindProject("http://localhost:8080");
        Assert.NotNull(urlProject);

        Assert.Equal(new Guid("c17f1c4c-4ab3-7d34-156f-dc53f03b9e5e"), urlProject.Id);

        SolutionProjectModel? fileProject = solution.FindProject(Path.Join("TestProjectRoot", "TestProjectRoot.csproj"));
        Assert.NotNull(fileProject);

        Assert.Equal(new Guid("a45c0ecb-b0af-1b66-6ffe-01df39badd75"), fileProject.Id);

        SolutionFolderModel? subFolder = solution.FindFolder("/Apps/Native/");
        Assert.NotNull(subFolder);

        Assert.Equal(new Guid("5d60b9be-f9d7-5599-c0c3-1663183ca464"), subFolder.Id);
    }
}
