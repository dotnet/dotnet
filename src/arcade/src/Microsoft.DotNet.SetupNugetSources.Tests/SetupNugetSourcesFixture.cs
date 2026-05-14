// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.SetupNugetSources.Tests
{
    /// <summary>
    /// xUnit fixture that prepares a temporary repository root containing the
    /// scaffolded files (global.json + eng/common scripts) copied from the
    /// build output's RepoScaffold directory. Shared per test class.
    /// </summary>
    public class SetupNugetSourcesFixture : IDisposable
    {
        public string RepoRoot { get; }
        public ScriptRunner ScriptRunner { get; }

        public SetupNugetSourcesFixture()
        {
            var scaffoldRoot = Path.Combine(AppContext.BaseDirectory, "RepoScaffold");
            if (!Directory.Exists(scaffoldRoot))
            {
                throw new InvalidOperationException($"Expected scaffold directory not found: {scaffoldRoot}");
            }

            RepoRoot = Path.Combine(Path.GetTempPath(), "SetupNugetSourcesTestRepo", Guid.NewGuid().ToString());
            CopyDirectory(scaffoldRoot, RepoRoot);

            ScriptRunner = new ScriptRunner(RepoRoot);
        }

        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            foreach (var dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(sourceDir, dir);
                Directory.CreateDirectory(Path.Combine(destinationDir, relative));
            }

            foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(sourceDir, file);
                var destPath = Path.Combine(destinationDir, relative);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
                File.Copy(file, destPath, overwrite: true);
            }
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(RepoRoot))
                {
                    Directory.Delete(RepoRoot, recursive: true);
                }
            }
            catch
            {
                // Best effort cleanup.
            }
        }
    }
}
