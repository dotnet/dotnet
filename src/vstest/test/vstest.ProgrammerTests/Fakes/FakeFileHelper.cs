// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeFileHelper : IFileHelper
{
    public FakeFileHelper(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public List<FakeFile> Files { get; } = new();
    public FakeErrorAggregator FakeErrorAggregator { get; }

    public void CopyFile(string sourcePath, string destinationPath)
    {
        throw new NotImplementedException();
    }

    public DirectoryInfo CreateDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public void Delete(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteDirectory(string directoryPath, bool recursive)
    {
        throw new NotImplementedException();
    }

    public void DeleteEmptyDirectroy(string directoryPath)
    {
        throw new NotImplementedException();
    }

    public bool DirectoryExists(string? path)
    {
        TPDebug.Assert(path is not null, "path is null");
        // TODO: Check if any file has the directory in name. This will improve.
        var directoryExists = Files.Select(f => Path.GetDirectoryName(f.Path)).Any(p => p != null && p.StartsWith(path!));
        return directoryExists;
    }

    public IEnumerable<string> EnumerateFiles(string directory, SearchOption searchOption, params string[]? endsWithSearchPatterns)
    {
        Func<FakeFile, string, bool> predicate = searchOption == SearchOption.TopDirectoryOnly
            ? (f, dir) => Path.GetDirectoryName(f.Path) == dir
            : (f, dir) => Path.GetDirectoryName(f.Path)!.Contains(dir);

        var files = Files.Where(f => predicate(f, directory)).Select(f => f.Path).ToList();
        return files;
    }

    public bool Exists(string? path)
    {
        throw new NotImplementedException();
    }

    public string GetCurrentDirectory()
    {
        throw new NotImplementedException();
    }

    public FileAttributes GetFileAttributes(string path)
    {
        throw new NotImplementedException();
    }

    public long GetFileLength(string path)
    {
        throw new NotImplementedException();
    }

    public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public Version GetFileVersion(string path)
    {
        throw new NotImplementedException();
    }

    public string GetFullPath(string path)
    {
        throw new NotImplementedException();
    }

    public Stream GetStream(string filePath, FileMode mode, FileAccess access = FileAccess.ReadWrite)
    {
        throw new NotImplementedException();
    }

    public Stream GetStream(string filePath, FileMode mode, FileAccess access, FileShare share)
    {
        throw new NotImplementedException();
    }

    public string GetTempPath()
    {
        throw new NotImplementedException();
    }

    public void MoveFile(string sourcePath, string destinationPath)
    {
        throw new NotImplementedException();
    }

    public void WriteAllTextToFile(string filePath, string content)
    {
        throw new NotImplementedException();
    }

    internal void AddFakeFile<T>(T file) where T : FakeFile
    {
        if (Files.Any(f => f.Path.Equals(file.Path, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Fake file '{file.Path}' already exists.");
        }

        Files.Add(file);
    }

    internal T GetFakeFile<T>(string path) where T : FakeFile
    {
        var matchingFiles = Files.Where(f => f.Path == path).ToList();
        if (matchingFiles.Count == 0)
            throw new FileNotFoundException($"Fake file {path}, was not found. Check if file was previously added to FakeFileHelper.");

        // TODO: The public collection of files should probably be made readonly / immutable, and internally be made a concurrent dictionary, because it does not make
        // sense to have more than 1 file object with the same name, and we check for that in AddFakeFile anyway.
        if (matchingFiles.Count > 1)
            throw new InvalidOperationException($"Fake file {path}, exists more than once. Are you modifying the Files collection in FakeFileHelper manually?");

        var file = matchingFiles.Single();
        if (file is not T result)
            throw new InvalidOperationException($"Fake file {path}, was supposed to be a {typeof(T)}, but was {file.GetType()}.");

        return result;
    }
}
