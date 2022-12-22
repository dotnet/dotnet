// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.IO
{
    /// <summary>
    /// Represents a temporary file. Creating an instance creates a file at the specified path,
    /// and disposing the instance deletes the file.
    /// </summary>
    public sealed class TempFile : IDisposable
    {
        /// <summary>Gets the created file's path.</summary>
        public string Path { get; }

        public TempFile(string path)
        {
            Path = path;
        }

        ~TempFile() => DeleteFile();

        public static TempFile Create([CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            return new TempFile(GetFilePath($"{IO.Path.GetRandomFileName()}_{memberName}_{lineNumber}"));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DeleteFile();
        }

        private void DeleteFile()
        {
            try { File.Delete(Path); }
            catch { /* Ignore exceptions on disposal paths */ }
        }

        private static string GetFilePath(string fileName)
        {
            string directory = IO.Path.Combine(IO.Path.GetTempPath(), IO.Path.GetRandomFileName());
            Directory.CreateDirectory(directory);
            return IO.Path.Combine(directory, fileName);
        }
    }
}
