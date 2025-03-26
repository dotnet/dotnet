// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

namespace Utilities;

/// <summary>
/// Helper methods to be included in sln/slnx tests.
/// </summary>
internal static class SlnTestHelper
{
    /// <summary>
    /// We do not support mono, but xunit testing tries to run the .NET Framework tests on mono when running on Linux/Mac.
    /// </summary>
#if NETFRAMEWORK
    internal static bool IsMono = Environment.OSVersion.Platform != PlatformID.Win32NT;
#else
    internal static bool IsMono = false;
#endif

    /// <summary>
    /// Converts the model to it's file contents using the specified serializer.
    /// </summary>
    internal static async Task<FileContents> ToLinesAsync<T>(this SolutionModel model, ISolutionSingleFileSerializer<T> serializer, int bufferSize = 1024 * 1024)
    {
        byte[] buffer = new byte[bufferSize];
        using (MemoryStream memoryStream = new MemoryStream(buffer))
        {
            await serializer.SaveAsync(memoryStream, model, CancellationToken.None);
        }

        return buffer.ToLines();
    }

    /// <summary>
    /// Save the model using the serializer then reload it into a new model.
    /// </summary>
    internal static async Task<(SolutionModel Model, FileContents Contents)> SaveAndReopenModelAsync<T>(
        ISolutionSingleFileSerializer<T> serializer,
        SolutionModel oldModel,
        int bufferSize = 1024 * 1024)
    {
        byte[] memoryBuffer = new byte[bufferSize];
        using (MemoryStream saveStream = new MemoryStream(memoryBuffer))
        using (MemoryStream openStream = new MemoryStream(memoryBuffer))
        {
            await serializer.SaveAsync(saveStream, oldModel, CancellationToken.None);

            // Use the length of the save stream to set the length of the open stream.
            openStream.SetLength(saveStream.Position);

            FileContents slnxContents = saveStream.ToLines();

            SolutionModel newModel = await serializer.OpenAsync(openStream, CancellationToken.None);
            return (newModel, slnxContents);
        }
    }

    internal static async Task<(SolutionModel Model, FileContents SlnxContents)> ThruSlnxStreamAsync(SolutionModel model, int bufferSize)
    {
        ISerializerModelExtension? originalExtension = model.SerializerExtension;

        // When converting to slnx, the order of projects will change (since they are grouped by folders).
        // This captures the order they are in for the .sln so it can be restored to get rid of diff noise.
        IReadOnlyList<Guid> originalOrder = model.SolutionItems.ToArray(x => x.Id);

        // Keep info that isn't serialized.
        Version? vsVersion = model.VisualStudioProperties.Version;
        Version? minVersion = model.VisualStudioProperties.MinimumVersion;
        string? openWith = model.VisualStudioProperties.OpenWith;
        Guid? solutionGuid = model.VisualStudioProperties.SolutionId;
        Dictionary<string, Guid> itemGuids = model.SolutionItems.ToDictionary(x => x.ItemRef, x => x.Id);
        HashSet<string> cpsProjects = new HashSet<string>(model.SolutionProjects.Where(IsCpsProject).Select(x => x.FilePath));
        SolutionPropertyBag? sharedProject = model.FindProperties(SectionName.SharedMSBuildProjectFiles);

        model.TrimVisualStudioProperties();

        (model, FileContents slnxContents) = await SaveAndReopenModelAsync(SolutionSerializers.SlnXml, model, bufferSize);

        foreach (KeyValuePair<string, Guid> projectGuid in itemGuids)
        {
            FindItem(model, projectGuid.Key).Id = projectGuid.Value;
        }

        foreach (SolutionProjectModel project in model.SolutionProjects)
        {
            if (cpsProjects.Contains(project.FilePath))
            {
                SetCpsProject(project);
            }
        }

        if (sharedProject is not null)
        {
            Assert.True(model.AddSlnProperties(sharedProject));
        }

        // Restore info that isn't serialized to make diff match.
        model.VisualStudioProperties.Version = vsVersion;
        model.VisualStudioProperties.MinimumVersion = minVersion;
        model.VisualStudioProperties.SolutionId = solutionGuid;
        model.VisualStudioProperties.OpenWith = openWith;

        // This is hacky, but need to preserve original order to make test diff work.
        List<SolutionItemModel> itemsHack = (List<SolutionItemModel>)model.SolutionItems;
        List<SolutionProjectModel> projectsHack = (List<SolutionProjectModel>)model.SolutionProjects;
        List<SolutionFolderModel> foldersHack = (List<SolutionFolderModel>)model.SolutionFolders;

        itemsHack.Sort((a, b) => originalOrder.IndexOf(a.Id).CompareTo(originalOrder.IndexOf(b.Id)));
        projectsHack.Sort((a, b) => originalOrder.IndexOf(a.Id).CompareTo(originalOrder.IndexOf(b.Id)));
        foldersHack.Sort((a, b) => originalOrder.IndexOf(a.Id).CompareTo(originalOrder.IndexOf(b.Id)));

        // Rehydrate some expected lost information when converting.
        model.SerializerExtension = originalExtension;
        return (model, slnxContents);

        static SolutionItemModel FindItem(SolutionModel solution, string itemRef)
        {
            return solution.SolutionItems.FindByItemRef(itemRef) ??
                throw new InvalidOperationException($"Project {itemRef} not found!");
        }

        static bool IsCpsProject(SolutionProjectModel project)
        {
            return
                project.TypeId == new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556") ||
                project.TypeId == new Guid("778DAE3C-4631-46EA-AA77-85C1314464D9") ||
                project.TypeId == new Guid("6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705");
        }

        static void SetCpsProject(SolutionProjectModel project)
        {
            if (project.TypeId == new Guid("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC"))
            {
                project.Type = "Common C#";
            }
            else if (project.TypeId == new Guid("F184B08F-C81C-45F6-A57F-5ABD9991F28F"))
            {
                project.Type = "Common VB";
            }
            else if (project.TypeId == new Guid("F2A71F9B-5D33-465A-A702-920D77279786"))
            {
                project.Type = "Common F#";
            }
        }
    }

    /// <summary>
    /// Starts with a slnx solution, modifies the model and verifies it matches the expected solution file.
    /// </summary>
    /// <param name="modifyModel">Action to modify the model.</param>
    /// <param name="original">Input slnx.</param>
    /// <param name="expected">Expected output slnx.</param>
    internal static Task ValidateModifiedSolutionAsync(Action<SolutionModel> modifyModel, ResourceStream original, ResourceStream expected)
    {
        return ValidateModifiedSolutionAsync(modifyModel, original, expected, SolutionSerializers.SlnXml);
    }

    /// <summary>
    /// Starts with a solution, modifies the model and verifies it matches the expected solution file.
    /// </summary>
    /// <param name="modifyModel">Action to modify the model.</param>
    /// <param name="original">Input solution.</param>
    /// <param name="expected">Expected output solution.</param>
    /// <param name="serializer">Serializer to use.</param>
    internal static async Task ValidateModifiedSolutionAsync<T>(Action<SolutionModel> modifyModel, ResourceStream original, ResourceStream expected, ISolutionSingleFileSerializer<T> serializer)
    {
        // Open the Model from stream.
        SolutionModel model = await serializer.OpenAsync(original.Stream, CancellationToken.None);
        AssertNotTarnished(model);

        modifyModel(model);

        // Save the Model back to stream.
        FileContents reserializedSolution = await ToLinesAsync(model, serializer);

        AssertSolutionsAreEqual(expected.ToLines(), reserializedSolution);
    }

    internal static Encoding GetSlnEncoding(SolutionModel model)
    {
        ISerializerModelExtension<SlnV12SerializerSettings>? slnExt = model.SerializerExtension as ISerializerModelExtension<SlnV12SerializerSettings>;

        // Expected SLN serializer for encoding.
        Assert.NotNull(slnExt);

        Encoding? encoding = slnExt.Settings.Encoding;

        Assert.NotNull(encoding);
        return encoding;
    }

    internal static void AssertSolutionsAreEqual(
        FileContents expectedSln,
        FileContents actualSln)
    {
        int count = Math.Min(expectedSln.Lines.Count, actualSln.Lines.Count);
        for (int i = 0; i < count; i++)
        {
            string expectedLine = expectedSln.Lines[i];
            string actualLine = actualSln.Lines[i];

            // Don't build the error message if the lines match.
            if (string.Equals(expectedLine, actualLine, StringComparison.Ordinal))
            {
                continue;
            }

            // Find the index where the strings do not match
            StringBuilder pointer = new StringBuilder(expectedLine.Length + 3);
            int minLength = Math.Min(expectedLine.Length, actualLine.Length);
            for (int sameCount = 0; sameCount < minLength && expectedLine[sameCount] == actualLine[sameCount]; sameCount++)
            {
                _ = pointer.Append(expectedLine[sameCount] == '\t' ? '\t' : '-');
            }

            _ = pointer.Append("^^");

            Assert.Fail(
                $"""
                Solution lines #{i + 1} do not match.
                {expectedLine}
                {actualLine}
                {pointer}
                EXPECTED:
                {expectedSln.FullString}
                ACTUAL:
                {actualSln.FullString}
                """);
        }

        string expectedSlnFull = expectedSln.FullString;
        if (Environment.NewLine != "\r\n")
        {
            expectedSlnFull = expectedSlnFull.Replace("\r\n", Environment.NewLine);
        }

        Assert.Equal(expectedSlnFull, actualSln.FullString);
    }

    // This only works for SLNX right now.
    internal static void AssertNotTarnished(SolutionModel model)
    {
        Assert.NotNull(model.SerializerExtension);
        Assert.False(model.SerializerExtension.Tarnished);
    }

    internal static void TryDeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch
        {
            // Ignore any exceptions.
        }
    }

    internal static FileContents ToLines(this byte[] buffer)
    {
        int length = Array.IndexOf(buffer, (byte)0);
        using (MemoryStream stream = new MemoryStream(buffer))
        {
            stream.SetLength(length);
            return stream.ToLines();
        }
    }

    internal static FileContents ToLines(this ResourceStream resource)
    {
        return resource.Stream.ToLines();
    }

    // Saves the resource to a temp file and returns the path.
    // Used if a test needs to validate reading from disk instead of stream.
    internal static string SaveResourceToTempFile(this ResourceStream resource)
    {
        string filePath = Path.ChangeExtension(Path.GetTempFileName(), resource.Name);

        try
        {
            using (FileStream stream = File.OpenWrite(filePath))
            {
                resource.Stream.CopyTo(stream);
                stream.SetLength(stream.Position);
            }
        }
        catch
        {
            TryDeleteFile(filePath);
            throw;
        }

        return filePath;
    }

    internal static bool TryGetSettings<T>(this SolutionModel model, out T? settings)
    {
        if (model.SerializerExtension is ISerializerModelExtension<T> serializerModelExtension &&
            serializerModelExtension.Settings is not null)
        {
            settings = serializerModelExtension.Settings;
            return true;
        }

        settings = default;
        return false;
    }

    private static FileContents ToLines(this Stream stream)
    {
        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true);
        string fullString = reader.ReadToEnd();
        stream.Position = 0;
        List<string> lines = new List<string>(1024);
        while (reader.ReadLine() is string line)
        {
            lines.Add(line);
        }

        stream.Position = 0;

        return new FileContents(fullString, lines);
    }
}
