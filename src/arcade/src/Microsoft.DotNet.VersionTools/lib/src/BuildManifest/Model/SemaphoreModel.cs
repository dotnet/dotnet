// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class SemaphoreModel
    {
        public const string BuildSemaphorePath = "build.semaphore";

        /// <summary>
        /// The path within the manifest directory where this semaphore is stored. Subdirectories
        /// are delimited by '/'.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        public string BuildId { get; set; }

        public string ToFileContent()
        {
            return BuildId + "\n";
        }

        public static SemaphoreModel Parse(string path, string fileContent)
        {
            return new SemaphoreModel
            {
                Path = path,
                BuildId = fileContent.Substring(0, fileContent.IndexOf('\n'))
            };
        }
    }
}
